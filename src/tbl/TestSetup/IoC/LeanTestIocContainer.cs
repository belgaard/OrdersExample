using System;
using System.Collections.Generic;
using LeanTest.Core.ExecutionHandling;
using Microsoft.Extensions.DependencyInjection;

namespace Tbl.TestDouble.TestSetup.IoC
{
    /// <summary>This is used in order to integrate LeanTest.Net with your preferred IoC container, in this case
    /// the .Net Core built-in IoC container.</summary>
    /// <remarks>See more details in teh coding guidelines on the wiki: https://wiki/display/OpenAPI/Lean+Test+Coding+Guidelines#LeanTestCodingGuidelines-IntegratingLeanTest.NetwithyourPreferredIoCContainer</remarks>
    public class LeanTestIocContainer : IIocContainer
    {
        private readonly IServiceProvider _serviceProvider;
        public LeanTestIocContainer(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public T Resolve<T>() where T : class => _serviceProvider.GetRequiredService<T>();
        public T TryResolve<T>() where T : class => _serviceProvider.GetService<T>();
        public IEnumerable<T> TryResolveAll<T>() where T : class => _serviceProvider.GetServices<T>();
    }
}
