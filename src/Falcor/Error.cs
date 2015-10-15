using System;

namespace Falcor
{
    public sealed class Error : FalcorValue
    {
        private readonly string _error;

        public Error(string error)
        {
            _error = error;
        }

        public override string AsError() => _error;

        public override bool IsValue()
        {
            throw new NotImplementedException();
        }

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        protected override ValueType ValueType { get; } = ValueType.Error;
    }
}