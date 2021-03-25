using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Grpc.Core;
using Tbl.TestDouble.Proto;
using Tbl.TestDouble.TestSetup;

namespace Tbl.TestDouble.Services
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated in the Startup class")]
    public class LeanTestService : Proto.LeanTest.LeanTestBase
    {
        private readonly WithDater _withDater;

        public LeanTestService(WithDater withDater) => _withDater = withDater;

        public override async Task<LeanTestResult> CreateContextBuilder(CreateContextBuilderRequest request, ServerCallContext context)
        {
            _withDater.CreateContextBuilder();
            return await Task.FromResult(new LeanTestResult { Status = 0 });
        }

        public override async Task<LeanTestResult> WithData(WithDataRequest request, ServerCallContext context)
        {
            _withDater.WithData(request);
            return await Task.FromResult(new LeanTestResult { Status = 0 });
        }

        public override async Task<LeanTestResult> Build(BuildRequest request, ServerCallContext context)
        {
            _withDater.Build();
            return await Task.FromResult(new LeanTestResult { Status = 0 });
        }

        public override async Task<LeanTestResult> WithClearDataStore(WithClearDataStoreRequest request, ServerCallContext context)
        {
            _withDater.WithClearDataStore(request);
            return await Task.FromResult(new LeanTestResult { Status = 0 });
        }

        /// <summary>Uses a scenario to do the equivalent of a series of WithData().</summary>
        public override async Task<LeanTestResult> WithScenarioData(WithScenarioDataRequest request, ServerCallContext context)
        {
            _withDater.WithScenarioData(request.ScenarioName);

            return await Task.FromResult(new LeanTestResult { Status = 0 });
        }
    }
}