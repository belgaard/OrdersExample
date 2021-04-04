using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanTest;
using LeanTest.Core.ExecutionHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeanTest.MSTest;
using TestScenarioIdAttribute = LeanTest.Attribute.TestScenarioIdAttribute;

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

        [TestMethod, TestScenarioId("InputValidation")]
        public async Task PostOrderMustBuyWhenAssetIsTradable()
        {
            // Declare BC:
            _contextBuilder
                .WithData(TestData.TradableAsset)
                .Build();

            HttpResponseMessage actual = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.ValidBuyOrder, Encoding.UTF8, "application/json"));

            Assert.IsTrue(actual.IsSuccessStatusCode);
        }
    }
}
