using LeanTest;
using LeanTest.Core.ExecutionHandling;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.L0Tests.TestSetup.IoC;

namespace Orders.L0Tests.TestSetup
{
    /// <summary>Does the setup which must must be done consistently across all tests in the assembly.</summary>
    [TestClass]
    public static class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            // The .NET Core web application factory is documented here
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1?view=aspnetcore-5.0
            static WebApplicationFactory<Startup> FactoryFactory()
            {
                var factory = new WebApplicationFactory<Startup>();
                factory = factory.WithWebHostBuilder(builder =>
                    builder
                        .ConfigureTestServices(L0CompositionRootForTest.Initialize));
 
                return factory;
            }
            AspNetCoreContextBuilderFactory.Initialize(FactoryFactory, provider => new IocContainer(provider));
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup() => ContextBuilderFactory.Cleanup();
    }
}