using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanTest.Core.ExecutionHandling;
using LeanTest.Xunit;
using Xunit;
using Orders.L0Tests.Mocks;
using Orders.Orders;
using Orders.SharedDomain;
using Xunit.Abstractions;
using TestScenarioIdAttribute = LeanTest.Attribute.TestScenarioIdAttribute;
using Microsoft.AspNetCore.Mvc.Testing;
using Orders.L0Tests.TestSetup;
using Orders.L0Tests.TestSetup.IoC;
using System;

namespace Orders.L0Tests
{
    public sealed class TestOrders : IDisposable
    {
        private readonly ContextBuilder _contextBuilder;
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _target;
        private readonly IQtePlacedOrderReader _qtePlacedOrderReader;

        public TestOrders(ITestOutputHelper output)
        {
            var testContext = new TestContext(output);
            _factory = new OrdersExampleWebApplication().Factory;
            _contextBuilder = new ContextBuilder(new IocContainer(_factory.Services))
                .RegisterAttributes(testContext);
            _target = _factory.CreateClient();
            _qtePlacedOrderReader = _contextBuilder.GetInstance<IQtePlacedOrderReader>();
        }

        /// <summary>The factory is not automatically disposed.</summary>
        public void Dispose() => _factory.Dispose();

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
            Assert.Contains("not allowed to trade", actual.ErrorInfo.ErrorCode);
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
            Assert.Contains("not allowed to trade", actual.ErrorInfo.ErrorCode);
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidId()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidId, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.Contains("Invalid Id", actual.ErrorInfo.ErrorCode);
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidPrice()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidPrice, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.Contains("Invalid price", actual.ErrorInfo.ErrorCode);
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidAmount()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidAmount, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.Contains("Invalid amount", actual.ErrorInfo.ErrorCode);
        }

        [Fact, TestScenarioId("Input")]
        public async Task PostOrderMustReportErrorWhenInvalidDuration()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidDuration, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.Contains("Invalid duration", actual.ErrorInfo.ErrorCode);
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
            Assert.Contains("Distance", actual.ErrorInfo.ErrorCode);
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenStepNotThere()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.StepNotThere, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.Contains("Step", actual.ErrorInfo.ErrorCode);
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenInvalidDistance()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidDistance, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.Contains("Distance", actual.ErrorInfo.ErrorCode);
        }

        [Fact, TestScenarioId("AdvancedInput")]
        public async Task PostOrderMustReportErrorWhenInvalidStep()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.RelatedOrders.InvalidStep, Encoding.UTF8, "application/json"));

            Assert.False(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.Contains("Step", actual.ErrorInfo.ErrorCode);
        }
    }
}
