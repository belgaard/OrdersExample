using Orders.L0Tests.TestSetup.IoC;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Orders.L0Tests.TestSetup
{
    /// <summary>Does the setup which must must be done consistently across all tests in the assembly.</summary>
    public class OrdersExampleWebApplication
    {
        public WebApplicationFactory<Startup> Factory {get;}

        public OrdersExampleWebApplication()
        {
            Factory = new WebApplicationFactory<Startup>();
            Factory = Factory.WithWebHostBuilder(builder =>
                builder
                    .ConfigureTestServices(L0CompositionRootForTest.Initialize));
        }
    }
}