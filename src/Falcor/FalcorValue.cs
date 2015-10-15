using System;

namespace Falcor
{
    public abstract class FalcorValue : FalcorNode
    {
        public override bool IsValue() => false;
        public override FalcorValue AsValue() => this;
        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree) => value(this);
        protected abstract ValueType ValueType { get; }
        public bool IsAtom => ValueType == ValueType.Atom;
        public bool IsRef => ValueType == ValueType.Ref;
        public bool IsErrror => ValueType == ValueType.Error;
        public virtual  FalcorPath AsRef() => null;
        public virtual string AsError() => null;
    }
}