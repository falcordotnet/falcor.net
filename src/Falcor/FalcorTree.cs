using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class FalcorTree : FalcorNode
    {
        public FalcorTree(IDictionary<SimpleKey, FalcorNode> values)
        {
            Children = new ConcurrentDictionary<SimpleKey, FalcorNode>(values);
        }


        public FalcorTree()
        {
            Children = new ConcurrentDictionary<SimpleKey, FalcorNode>();
        }

        public ConcurrentDictionary<SimpleKey, FalcorNode> Children { get; }
        public override bool IsValue => false;
        public override FalcorTree AsTree() => this;
        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree) => tree(this);

        public override JToken ToJson()
        {
            throw new NotImplementedException();
        }

    }
}