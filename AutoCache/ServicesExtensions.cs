using AutoCache;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Adds a scoped service of the type specified in TService with an implementation behind a proxy with cache
        /// </summary>
        /// <typeparam name="TInterface">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">A reference to this instance after the operation has completed.</param>
        public static void AddCachedScoped<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {

            if(!services.Any(s => s.ServiceType == typeof(IMemoryCache)))
                services.AddMemoryCache();

            // Se não tiver sido adicionado ainda, injeta o gerador de proxy
            services.TryAddSingleton(new ProxyGenerator());

            // Se não tiver sido adicionado ainda, injeta o interceptador
            services.TryAddSingleton<CacheInterceptor>();
            
            // Se não tiver sido adicionada ainda, injeta a implementação original
            services.TryAddScoped<TImplementation>();


            // Adiciona o proxy
            services.AddScoped(serviceProvider =>
            {
                // Busca o gerador de proxy
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();

                // Busca a implementação original
                var original = serviceProvider.GetRequiredService<TImplementation>();

                // Busca a implementação do cache
                var cacheInterceptor = serviceProvider.GetService<CacheInterceptor>();

                // Retorna um proxy (que faz o cache) para o objeto original
                return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(original, cacheInterceptor);
            });
        }
        
        /// Adds a transient service of the type specified in TService with an implementation behind a proxy with cache
        /// </summary>
        /// <typeparam name="TInterface">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">A reference to this instance after the operation has completed.</param>
        public static void AddCachedTransient<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {

            if (!services.Any(s => s.ServiceType == typeof(IMemoryCache)))
                services.AddMemoryCache();

            // Se não tiver sido adicionado ainda, injeta o gerador de proxy
            services.TryAddSingleton(new ProxyGenerator());

            // Se não tiver sido adicionado ainda, injeta o interceptador
            services.TryAddSingleton<CacheInterceptor>();

            // Se não tiver sido adicionada ainda, injeta a implementação original
            services.TryAddTransient<TImplementation>();


            // Adiciona o proxy
            services.AddTransient(serviceProvider =>
            {
                // Busca o gerador de proxy
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();

                // Busca a implementação original
                var original = serviceProvider.GetRequiredService<TImplementation>();

                // Busca a implementação do cache
                var cacheInterceptor = serviceProvider.GetService<CacheInterceptor>();

                // Retorna um proxy (que faz o cache) para o objeto original
                return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(original, cacheInterceptor);
            });
        }

        /// <summary>
        /// Adds a singleton service of the type specified in TService with an implementation behind a proxy with cache
        /// </summary>
        /// <typeparam name="TInterface">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">A reference to this instance after the operation has completed.</param>
        public static void AddCachedSingleton<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {

            if (!services.Any(s => s.ServiceType == typeof(IMemoryCache)))
                services.AddMemoryCache();

            // Se não tiver sido adicionado ainda, injeta o gerador de proxy
            services.TryAddSingleton(new ProxyGenerator());

            // Se não tiver sido adicionado ainda, injeta o interceptador
            services.TryAddSingleton<CacheInterceptor>();

            // Se não tiver sido adicionada ainda, injeta a implementação original
            services.TryAddSingleton<TImplementation>();


            // Adiciona o proxy
            services.AddSingleton(serviceProvider =>
            {
                // Busca o gerador de proxy
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();

                // Busca a implementação original
                var original = serviceProvider.GetRequiredService<TImplementation>();

                // Busca a implementação do cache
                var cacheInterceptor = serviceProvider.GetService<CacheInterceptor>();

                // Retorna um proxy (que faz o cache) para o objeto original
                return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(original, cacheInterceptor);
            });
        }
    }
}
