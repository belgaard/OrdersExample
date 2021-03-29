using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanTest;
using LeanTest.Core.ExecutionHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeanTest.MSTest;

namespace Orders.L0Tests
{
    [TestClass]
    public class TestOrders
    {
        private ContextBuilder _contextBuilder;
        private HttpClient _target;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _contextBuilder = ContextBuilderFactory.CreateContextBuilder()
                .RegisterAttributes(TestContext);

            _target = _contextBuilder.GetHttpClient();
        }
        [TestMethod]
        public async Task PlaceOrderMustReportErrorWhenInputIsInvalid()
        {
            HttpResponseMessage actual = await _target.PostAsync("/orders/place-order", new StringContent("{}", Encoding.UTF8, "application/json"));

            // TODO: Make the test fail for the right reason, e.g. let it test the 4 invalid input combinations.
        }
    }
}
