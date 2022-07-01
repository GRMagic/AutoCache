using AutoCache.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace AutoCache.Tests
{
    public class Tests
    {

        [Fact(DisplayName = "When calling a function more times, the second time should produce an equal result")]
        [Trait("Type", "Cache")]
        public async Task InCachedScoped_CallCachedFunction_ResultEquals()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddCachedScoped<IService, Service>();

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetService<IService>()!;

            var param = 1000;

            // Act
            var firstResult = service.DoSomethingWithCache(param);
            var secondResult = service.DoSomethingWithCache(param);

            // Assert
            Assert.Equal(firstResult, secondResult);
        }

        [Fact(DisplayName = "When getting a scoped instance more times, the second time should return the same object")]
        [Trait("Type", "Live cycle")]
        public void InCachedScoped_InnerService_Same()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddCachedScoped<IService, Service>();

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();

            // Act
            var serviceA = scope.ServiceProvider.GetService<IService>()!;
            var serviceB = scope.ServiceProvider.GetService<IService>()!;

            // Assert
            Assert.Same(serviceA, serviceB);
            Assert.Equal(serviceA.InstanceId(), serviceB.InstanceId());
        }

        [Fact(DisplayName = "Different scopes should return different scoped instances")]
        [Trait("Type", "Live cycle")]
        public void InCachedScoped_InnerService_NotSame()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddCachedScoped<IService, Service>();

            var serviceProvider = services.BuildServiceProvider();
            var scopeA = serviceProvider.CreateScope();
            var scopeB = serviceProvider.CreateScope();

            // Act
            var serviceA = scopeA.ServiceProvider.GetService<IService>()!;
            var serviceB = scopeB.ServiceProvider.GetService<IService>()!;

            // Assert
            Assert.NotSame(serviceA, serviceB);
            Assert.NotEqual(serviceA.InstanceId(), serviceB.InstanceId());
        }

        [Fact(DisplayName = "Transient injections should return different instances")]
        [Trait("Type", "Live cycle")]
        public void InCachedTransient_InnerService_NotSame()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddCachedTransient<IService, Service>();

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();

            // Act
            var serviceA = scope.ServiceProvider.GetService<IService>()!;
            var serviceB = scope.ServiceProvider.GetService<IService>()!;

            // Assert
            Assert.NotSame(serviceA, serviceB);
            Assert.NotEqual(serviceA.InstanceId(), serviceB.InstanceId());
        }

        [Fact(DisplayName = "All instances should shared the same cache")]
        [Trait("Type", "Cache")]
        public void DifferentInstancesOfService_CallCachedFunction_SharedCache()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddCachedTransient<IService, Service>();

            var serviceProvider = services.BuildServiceProvider();
            
            var serviceA = serviceProvider.GetService<IService>()!;
            var serviceB = serviceProvider.GetService<IService>()!;

            var param = 1000;
            var resultA = serviceA.DoSomethingWithCache(param);

            // Act
            var resultB = serviceB.DoSomethingWithCache(param);

            // Assert
            Assert.NotSame(serviceA, serviceB);
            Assert.NotEqual(serviceA.InstanceId(), serviceB.InstanceId());
            Assert.Equal(resultA, resultB);
        }

        [Fact(DisplayName = "When getting a singleton instance more times, the second time should return the same object")]
        [Trait("Type", "Live cycle")]
        public void InCachedSingleton_InnerService_Same()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddCachedSingleton<IService, Service>();

            var serviceProvider = services.BuildServiceProvider();
            
            // Act
            var serviceA = serviceProvider.GetService<IService>()!;
            var serviceB = serviceProvider.GetService<IService>()!;

            // Assert
            Assert.Same(serviceA, serviceB);
            Assert.Equal(serviceA.InstanceId(), serviceB.InstanceId());
        }

        [Fact(DisplayName = "When calling a function after cache timeout, the second time should call inner function again")]
        [Trait("Type", "Cache")]
        public async Task InCachedSingleton_InnerService_Sameasdf()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddCachedTransient<IService, Service>();

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IService>()!;
            var param = 123;
            var result = service.DoSomethingWithCache(param);
            await Task.Delay(1000);

            // Act
            var actResult = service.DoSomethingWithCache(param);

            // Assert
            Assert.NotEqual(result, actResult);
        }

    }
}