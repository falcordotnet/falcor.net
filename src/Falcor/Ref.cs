using System;
using System.Collections;
using System.Collections.Generic;

namespace Falcor
{
    public sealed class Ref : FalcorValue, IEnumerable<KeySegment>
    {
        private readonly FalcorPath _path;

        public Ref(FalcorPath path)
        {
            _path = path;
        }

        public override bool IsValue()
        {
            throw new NotImplementedException();
        }

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        protected override ValueType ValueType { get; } = ValueType.Ref;

        public override FalcorPath AsRef() => _path;

        public IEnumerator<KeySegment> GetEnumerator()
        {
            return _path.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}