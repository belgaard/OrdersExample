using System;
using System.Collections.Generic;
using LeanTest.Core.ExecutionHandling;
using Microsoft.Extensions.DependencyInjection;

namespace Orders.L0Tests.TestSetup.IoC
{
    /// <summary>This is used in order to integrate LeanTest.Net with the preferred IoC container, in this case
    /// the .Net Core built-in IoC container.</summary>
    public class IocContainer : IIocContainer
    {
        private readonly IServiceProvider _serviceProvider;
        public IocContainer(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public T Resolve<T>() where T : class => _serviceProvider.GetRequiredService<T>();
        public T TryResolve<T>() where T : class => _serviceProvider.GetService<T>();
        public IEnumerable<T> TryResolveAll<T>() where T : class => _serviceProvider.GetServices<T>();
    }
}