using LeanTest.Mock;
using Microsoft.Extensions.DependencyInjection;
using Orders.ExternalDependencies;
using Orders.L0Tests.Mocks;

namespace Orders.L0Tests.TestSetup.IoC
{
    /// <summary>A composition root is where the dependency graph is initialized. In the test project, we build on the production code
    /// composition root, as defined in Startup.ConfigureServices(), by only overriding (i.e. by mocking) interfaces which represent truly
    /// external dependencies (we prefer to mock as little as possible).</summary>
    /// <remarks>
    /// See this for an explanation of the underlying principle behind composition roots: http://blog.ploeh.dk/2011/07/28/CompositionRoot/.
    /// See this for details on why we prefer to mock as little as possible: https://medium.com/swlh/should-you-unit-test-in-asp-net-core-793de767ac68
    /// </remarks>
    public static class L0CompositionRootForTest
    {
        public static void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterMockForData<IInstrumentCacheFacade, MockForDataInstrumentCacheFacade, Tradable>();
            serviceCollection.AddSingleton<ISessionTradesFacade, MockSessionTradesFacade>();
        }

        /// <summary>We prefer to use test doubles (mocks) which are primed with boundary values (TData). Boundary values are simply passed out via the
        /// production code interface (TInterface) overridden in Initialize above.</summary>
        /// <remarks>This way we avoid much test code, and thus avoid potential bugs. We simply accept that *the value is the boundary*.
        /// There is more on this topic here: https://belgaard.medium.com/why-dont-you-take-given-in-bdd-seriously-f168da29f1c. </remarks>
        /// <typeparam name="TInterface">The production interface to be overridden.</typeparam>
        /// <typeparam name="TImplementation">The implementation of the test double.</typeparam>
        /// <typeparam name="TData">The type of boundary value which the test double is to be primed with.</typeparam>
        /// <param name="serviceCollection">The production code IoC container in which to override TInterface.</param>
        private static void RegisterMockForData<TInterface, TImplementation, TData>(this IServiceCollection serviceCollection)
            where TImplementation : class, TInterface, IMockForData<TData>
            where TInterface : class
        {
            serviceCollection.AddSingleton<TImplementation>();
            serviceCollection.AddSingleton<TInterface>(x => x.GetRequiredService<TImplementation>());
            serviceCollection.AddSingleton<IMockForData<TData>>(x => x.GetRequiredService<TImplementation>());
        }
    }
}