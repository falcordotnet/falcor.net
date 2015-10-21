using System;
using System.Collections.Concurrent;

namespace Falcor
{
    public sealed class FalcorTree : FalcorNode
    {
        public ConcurrentDictionary<KeySegment, FalcorNode> Children { get; private set; }
        public override bool IsValue => false;
        public override FalcorTree AsTree() => this;
        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree) => tree(this);

        public FalcorTree()
        {
            Children = new ConcurrentDictionary<KeySegment, FalcorNode>();
        }
    }
}