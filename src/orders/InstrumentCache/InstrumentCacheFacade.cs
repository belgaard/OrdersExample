using Orders.ExternalDependencies;

namespace Orders.InstrumentCache
{
    class InstrumentCacheFacade : IInstrumentCacheFacade
    {
        public bool IsInstrumentTradableByUic(int uic)
        {
            // TODO: Call an InstrumentCache gRPC service!
            return true;
        }
    }
}