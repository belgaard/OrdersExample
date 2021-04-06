namespace Orders.ExternalDependencies
{
    public interface IInstrumentCacheFacade
    {
        bool IsInstrumentTradableByUic(int uic);
    }
}