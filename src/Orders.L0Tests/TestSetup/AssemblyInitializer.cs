using LeanTest;
using LeanTest.Core.ExecutionHandling;
using Orders.L0Tests.TestSetup.IoC;
using LeanTest.Xunit;
using Orders.L0Tests.TestSetup;

[assembly: AssemblyFixture(typeof(AssemblyInitializer))]
[assembly: Xunit.TestFramework("LeanTest.Xunit.XunitExtensions.XunitTestFrameworkWithAssemblyFixture", "LeanTest.Xunit")]

namespace Orders.L0Tests.TestSetup
{
    /// <summary>Does the setup which must must be done consistently across all tests in the assembly.</summary>
    public sealed class AssemblyInitializer
    {
        public AssemblyInitializer()
        {
            static ICreateContextBuilder FactoryFactory() =>
                new LeanTestWebApplicationFactory<Startup>(L0CompositionRootForTest.Initialize, provider => new IocContainer(provider));

            ContextBuilderFactory.Initialize(FactoryFactory);
        }
    }
}