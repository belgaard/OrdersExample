using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanTest;
using LeanTest.Core.ExecutionHandling;
using LeanTest.Xunit;
using Xunit;
using Orders.L0Tests.Mocks;
using Orders.Orders;
using Orders.SharedDomain;
using Xunit.Abstractions;
using TestScenarioIdAttribute = LeanTest.Attribute.TestScenarioIdAttribute;

namespace Orders.L0Tests
{
    public class TestOrders
    {
        private ContextBuilder _contextBuilder;
        private HttpClient _target;
        private IQtePlacedOrderReader _qtePlacedOrderReader;

        public TestOrders(ITestOutputHelper output)
        {
            var testContext = new TestContext(output);
            _contextBuilder = ContextBuilderFactory.CreateContextBuilder()
                .RegisterAttributes(testContext)
                .Build();

            _qtePlacedOrderReader = _contextBuilder.GetInstance<IQtePlacedOrderReader>();
            _target = _contextBuilder.GetHttpClient();
        }

        [Fact, TestScenarioId("Core")]
        public async Task PostOrderMustBuyWhenAssetIsTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.TradableAsset)
                .Build();

            HttpResponseMessage actual = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidBuy, Encoding.UTF8, "application/json"));

            Assert.True(actual.IsSuccessStatusCode);
            Assert.Equal((TestData.ValidInstrumentId, BuySell.Buy), _qtePlacedOrderReader.Query()); // Observe the difference between IC and the resulting state. 
        }

        [Fact, TestScenarioId("Core")]
        public async Task PostOrderMustSellWhenAssetIsTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.TradableAsset)
                .Build();

            HttpResponseMessage actual = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidSell, Encoding.UTF8, "application/json"));

            Assert.True(actual.IsSuccessStatusCode);
            Assert.Equal((TestData.ValidInstrumentId, BuySell.Sell), _qtePlacedOrderReader.Query()); // Observe the difference between IC and the resulting state. 
        }

        [Fact, TestScenarioId("Core")]
        public async Task PostOrderMustNotBuyWhenAssetIsNotTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.NonTradableAsset)
                .Build();

            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidBuy, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("not allowed to trade"));
        }

        [Fact, TestScenarioId("Core")]
        public async Task PostOrderMustNotSellWhenAssetIsNotTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.NonTradableAsset)
                .Build();

            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.ValidSell, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("not allowed to trade"));
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidId()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidId, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Invalid Id"));
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidPrice()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidPrice, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Invalid price"));
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidAmount()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidAmount, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Invalid amount"));
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidDuration()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidDuration, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Invalid duration"));
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenInvalidPositionId()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidPositionId, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenPositionIdNotThere()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.PositionIdNotThere, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenDistanceNotThere()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.DistanceNotThere, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Distance"));
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenStepNotThere()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.StepNotThere, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Step"));
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenInvalidDistance()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidDistance, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Distance"));
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenInvalidStep()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidStep, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.True(actual.ErrorInfo.ErrorCode.Contains("Step"));
        }
    }
}
