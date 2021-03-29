using LeanTest;
using LeanTest.Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Orders.L0Tests.TestSetup;
using Orders.L0Tests.TestSetup.IoC;

[assembly: AssemblyFixture(typeof(AssemblyInitializer))]
[assembly: Xunit.TestFramework("LeanTest.Xunit.XunitExtensions.XunitTestFrameworkWithAssemblyFixture", "LeanTest.Xunit")]

namespace Orders.L0Tests.TestSetup
{
    /// <summary>Does the setup which must must be done consistently across all tests in the assembly.</summary>
    public sealed class AssemblyInitializer
    {
        public AssemblyInitializer()
        {
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
    }
}
