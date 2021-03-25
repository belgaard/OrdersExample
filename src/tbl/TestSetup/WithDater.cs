using System;
using System.Text.RegularExpressions;
using LeanTest.Core.ExecutionHandling;
using LeanTest.JSon;
using Tbl.TestDouble.Proto;
using static Newtonsoft.Json.JsonConvert;

namespace Tbl.TestDouble.TestSetup
{
    /// <remarks>Note that this is a singleton which keep state across gRPC service invocations. In other words, there is but a single (initial) context
    /// across the service, so we assume that we have one caller (one test) at a time. This is by design. A single simulator is supposed to run in one environment, and its
    /// state is supposed to be setup to be consistent across all callers in that environment. Only the setup of the initial context must be done but once, we do
    /// not anticipate a requirement for parallel setup.</remarks>
    public class WithDater
    {
        private ContextBuilder _contextBuilder;
        private readonly Regex _dataNameRegEx = new(@"(?<type>^[^\.]*)\.(?<file>[^\.]*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly TestData.TestData TestDataReader = new();

        public void CreateContextBuilder() =>
            _contextBuilder = ContextBuilderFactory.CreateContextBuilder()
                // TODO: Pre-declare all types that we handle, so we can clear data in PreBuild:
                //.WithData<ClmDataFeedForexData>()
            ;

        /// <summary>WithData on the context builder.</summary>
        /// <param name="request">Deserialize to the data type part of <c>DataTypeAndName</c>. The data to deserialize is either <c>Data</c> or, if null,
        /// an embedded Json file with the name of <c>DataTypeAndName</c>.</param>
        public void WithData(WithDataRequest request)
        {
            GroupCollection dataNameGroups = _dataNameRegEx.Match(request.DataTypeAndName).Groups;
            string typeAsString = dataNameGroups["type"].Value;
            var type = GetType(typeAsString);
            string data = string.IsNullOrEmpty(request.Data) ? TestDataReader[type, $"{dataNameGroups["file"].Value}.json"] : request.Data;
            _contextBuilder
                .WithData(type, DeserializeObject(data, type));
        }

        /// <summary>
        /// We support the types found in the DataStructures namespace of the InitialContext assembly + DateTime from the System namespace.
        /// </summary>
        /// <param name="typeAsString"></param>
        /// <returns></returns>
        private static Type GetType(string typeAsString)
        {
            string typeString =
                $"Iit.PricingAsAService.Simulator.InitialContext.DataStructures.{typeAsString}, Iit.PricingAsAService.Simulator.InitialContext";
            var type = Type.GetType(typeString);
            if (type != null)
                return type;

            string typeStringSystem = $"System.{typeAsString}, mscorlib";
            type = Type.GetType(typeStringSystem);

            return type;
        }

        public void Build() => _contextBuilder.Build();

        public void WithClearDataStore(WithClearDataStoreRequest request) => _contextBuilder.WithClearDataStore();

        public void WithScenarioData(string scenarioName)
        {
            JsonDataCreatorForTest<ScenarioData> scenarioDataDataCreator = new();
            ScenarioData scenarioData = scenarioDataDataCreator.DeserializeFromJson(TestDataReader[$"{scenarioName}.json"]);
            foreach (ScenarioData.DataTypeAndName d in scenarioData.Data)
                _contextBuilder
                    .WithData(d.Type, DeserializeObject(TestDataReader[d.Type, d.Name], d.Type));
        }

        public record ScenarioData
        {
            public string Description { get; init; }
            public DataTypeAndName[] Data { get; init; }

            public record DataTypeAndName
            {
                public Type Type { get; init; } // Let deserialization do the reflection.
                public string Name { get; init; }
            }
        }
    }}
