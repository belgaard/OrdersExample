using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanTest.Core.ExecutionHandling;
using LeanTest.Xunit;
using LeanTest.JSon;
using Orders.L2Tests.Handlers;
using Orders.L2Tests.TestSetup;
using Xunit;
using Xunit.Abstractions;
using TestScenarioIdAttribute = LeanTest.Attribute.TestScenarioIdAttribute;

namespace Orders.L2Tests
{
    public class TestOrders
    {
        private readonly ContextBuilder _contextBuilder;
        private readonly HttpClient _target;
        private static TestData.TestData TestData => new();

        public TestOrders(ITestOutputHelper output)
        {
            var testContext = new TestContext(output);
            _contextBuilder = ContextBuilderFactory.CreateContextBuilder()
                .RegisterAttributes(testContext)
                .Build();

            _target = _contextBuilder.GetInstance<L2TestEnvironment>().HttpClient;
        }

        [Fact, TestScenarioId("Core")]
        public async Task PostOrderMustBuyWhenAssetIsTradable()
        {
            // Declare IC:
            _contextBuilder
                .WithData<Tradable>(TestData["TradableAsset"])
                .Build();

            HttpResponseMessage actual = await 
                _target.PostAsync("/orders/place-order", new StringContent(TestData["RelatedOrders.ValidBuy"], Encoding.UTF8, "application/json"));

            // It is sometimes possible to observe the difference between IC and the resulting state, even in L2 tests. For now, we don't.
            Assert.True(actual.IsSuccessStatusCode);
        }
    }
}
