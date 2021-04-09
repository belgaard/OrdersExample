using Microsoft.Extensions.DependencyInjection;

namespace Orders.L2Tests.TestSetup.IoC
{
    public class L2CompositionRootForTest
    {
        private readonly L2TestEnvironment _testEnvironment;

        public L2CompositionRootForTest(L2TestEnvironment testEnvironment) => _testEnvironment = testEnvironment;

        public IServiceCollection Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(_testEnvironment);
            //serviceCollection.AddSingleton(s => new ClientComponent.ClientComponent(new MockLogger())); // TODO: Proper logging!!!
            //serviceCollection.RegisterStateHandler<SimulatorHandler, WithDataRequest>()
            //    .WithDataForStateHandler<SimulatorHandler, DateTime>()
            //    .WithDataForStateHandler<SimulatorHandler, ClmDataFeedStocksData>()
            //    .WithInterface<SimulatorHandler, IDataStore>()
            //    .WithInterface<SimulatorHandler, SimulatorHandler.IDateTime>();

            return serviceCollection;
        }
    }
}