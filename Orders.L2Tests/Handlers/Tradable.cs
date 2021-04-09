using System;
using System.Collections.Generic;
using LeanTest.Core.ExecutionHandling;

namespace Orders.L2Tests.Handlers
{
    public record Tradable(in bool IsTradable, in int Uic);

    /// <summary>TODO: When production code starts calling the instruments gRPC code, we will handle Tradable here!</summary>
    public class TradableStateHandler : IStateHandler<Tradable>
    {
        private readonly Dictionary<int, bool> _tradablePerUic = new();

        public void WithData(Tradable data) => _tradablePerUic[data.Uic] = data.IsTradable;

        public void PreBuild() { }
        public void Build(Type type) { }
        public void PostBuild() { }
    }
}