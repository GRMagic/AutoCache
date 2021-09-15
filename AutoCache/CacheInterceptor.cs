using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace AutoCache
{
    sealed class CacheInterceptor : IInterceptor
    {
        private readonly IMemoryCache _cache;

        public CacheInterceptor(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        private static string GerarChaveCache(IInvocation invocation)
        {
            var chave = new
            {
                C = invocation.TargetType.AssemblyQualifiedName,
                M = invocation.Method.Name,
                A = invocation.Arguments.Select(a => a?.ToString())
            };
            return System.Text.Json.JsonSerializer.Serialize(chave);
        }

        public void Intercept(IInvocation invocation)
        {
            var cacheAttribute = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(CacheAttribute), false).FirstOrDefault() as CacheAttribute;

            if (cacheAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            var chave = GerarChaveCache(invocation);

            if (_cache.TryGetValue(chave, out object retorno))
            {
                invocation.ReturnValue = retorno;
            }
            else
            {
                invocation.Proceed();

                var options = new MemoryCacheEntryOptions()
                {
                    Priority = cacheAttribute.Priority
                };
                if (cacheAttribute.Seconds > 0)
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheAttribute.Seconds);
                if (cacheAttribute.SlidingSeconds > 0)
                    options.SlidingExpiration = TimeSpan.FromSeconds(cacheAttribute.SlidingSeconds);

                _cache.Set(chave, invocation.ReturnValue, options);
            }
        }
    }
}
