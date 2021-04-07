using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanTest;
using LeanTest.Core.ExecutionHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeanTest.MSTest;
using Orders.Orders;
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
        public async Task PostOrderMustReportErrorWhenInvalidId()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.InvalidId, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid Id"));
        }

        [TestMethod, TestScenarioId("InputValidation")]
        public async Task PostOrderMustReportErrorWhenInvalidPrice()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.InvalidPrice, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid price"));
        }

        [TestMethod, TestScenarioId("InputValidation")]
        public async Task PostOrderMustReportErrorWhenInvalidAmount()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.InvalidAmount, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid amount"));
        }

        [TestMethod, TestScenarioId("InputValidation")]
        public async Task PostOrderMustReportErrorWhenInvalidDuration()
        {
            // No IC to declare!
            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.InvalidDuration, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("Invalid duration"));
        }

        [TestMethod, TestScenarioId("CoreFunctionality")]
        public async Task PostOrderMustBuyWhenAssetIsTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.TradableAsset)
                .Build();

            HttpResponseMessage actual = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.ValidBuyOrder, Encoding.UTF8, "application/json"));

            Assert.IsTrue(actual.IsSuccessStatusCode);
        }

        [TestMethod, TestScenarioId("CoreFunctionality")]
        public async Task PostOrderMustNotBuyWhenAssetIsNotTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData(TestData.NonTradableAsset)
                .Build();

            HttpResponseMessage response = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData.ValidBuyOrder, Encoding.UTF8, "application/json"));

            Assert.IsFalse(response.IsSuccessStatusCode);
            var actual = await response.Content.ReadAsAsync<GenericOrderResponse>();
            Assert.IsTrue(actual.ErrorInfo.ErrorCode.Contains("not allowed to trade"));
        }
    }
}
