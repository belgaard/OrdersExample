using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanTest;
using LeanTest.Core.ExecutionHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeanTest.MSTest;
using Orders.L0Tests.Mocks;
using Orders.Orders;
using Orders.SharedDomain;
using TestScenarioIdAttribute = LeanTest.Attribute.TestScenarioIdAttribute;

namespace Orders.L0Tests
{
    [TestClass]
    public class TestOrders
    {
        private ContextBuilder _contextBuilder;
        private HttpClient _target;
        private IQtePlacedOrderReader _qtePlacedOrderReader;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            _contextBuilder = ContextBuilderFactory.CreateContextBuilder()
                .RegisterAttributes(TestContext);

            _qtePlacedOrderReader = _contextBuilder.GetInstance<IQtePlacedOrderReader>();
            _target = _contextBuilder.GetHttpClient();
        }

        [TestMethod, TestScenarioId("Core")]
        public async Task PostOrderMustBuyWhenAssetIsTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.TradableAsset)
                .Build();

            HttpResponseMessage actual = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidBuy, Encoding.UTF8, "application/json"));

            Assert.IsTrue(actual.IsSuccessStatusCode);
            Assert.AreEqual((TestData.ValidInstrumentId, BuySell.Buy), _qtePlacedOrderReader.Query()); // Observe the difference between IC and the resulting state. 
        }

        [TestMethod, TestScenarioId("Core")]
        public async Task PostOrderMustSellWhenAssetIsTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.TradableAsset)
                .Build();

            HttpResponseMessage actual = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidSell, Encoding.UTF8, "application/json"));

            Assert.IsTrue(actual.IsSuccessStatusCode);
            Assert.AreEqual((TestData.ValidInstrumentId, BuySell.Sell), _qtePlacedOrderReader.Query()); // Observe the difference between IC and the resulting state. 
        }

        [TestMethod, TestScenarioId("Core")]
        public async Task PostOrderMustNotBuyWhenAssetIsNotTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.NonTradableAsset)
                .Build();

            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidBuy, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("not allowed to trade"));
        }

        [TestMethod, TestScenarioId("Core")]
        public async Task PostOrderMustNotSellWhenAssetIsNotTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.NonTradableAsset)
                .Build();

            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidSell, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("not allowed to trade"));
        }

        [TestMethod, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidId()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidId, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid Id"));
        }

        [TestMethod, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidPrice()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidPrice, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid price"));
        }

        [TestMethod, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidAmount()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidAmount, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid amount"));
        }

        [TestMethod, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidDuration()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidDuration, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid duration"));
        }

        [TestMethod, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenInvalidPositionId()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidPositionId, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
        }
    }
}
