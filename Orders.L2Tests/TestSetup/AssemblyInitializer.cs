using System;
using LeanTest.Core.ExecutionHandling;
using Microsoft.Extensions.DependencyInjection;
using Orders.L2Tests.TestSetup.IoC;

namespace Orders.L2Tests.TestSetup
{
    /// <summary>Does the setup which must must be done consistently across all tests in the assembly.</summary>
    public sealed class AssemblyInitializer
    {
        public AssemblyInitializer()
        {
            static L2TestEnvironment CreateEnvironment() // So far, we restrict ourselves to a localhost environment.
            {
                L2TestEnvironment l2TestEnvironment = new();
                l2TestEnvironment.HttpClient.BaseAddress = new Uri("https://localhost:44327/");

                return l2TestEnvironment;
            }

            ContextBuilderFactory.Initialize(CleanContextMode.ReCreate, () =>
                new IocContainer(
                    new L2CompositionRootForTest(CreateEnvironment()).Initialize(new ServiceCollection()).BuildServiceProvider()));
        }
    }
}