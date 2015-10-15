using System;

namespace Falcor
{
    public sealed class Atom : FalcorValue
    {
        public override bool IsValue()
        {
            throw new NotImplementedException();
        }

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        protected override ValueType ValueType { get; }= ValueType.Atom;
    }
}