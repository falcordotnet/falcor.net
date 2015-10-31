using System;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class FalcorTree : FalcorNode
    {
        public FalcorTree()
        {
            Children = new ConcurrentDictionary<KeySegment, FalcorNode>();
        }

        public ConcurrentDictionary<KeySegment, FalcorNode> Children { get; private set; }
        public override bool IsValue => false;
        public override FalcorTree AsTree() => this;
        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree) => tree(this);

        public override JToken ToJToken()
        {
            throw new NotImplementedException();
        }
    }
}