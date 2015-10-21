using System;

namespace Falcor
{
    public sealed class Atom : FalcorValue
    {
        public override bool IsValue => true;

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        protected override ValueType ValueType { get; } = ValueType.Atom;
    }
}