using System;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class Error : FalcorValue
    {
        private readonly string _error;

        public Error(string error)
        {
            _error = error;
        }

        public override bool IsValue => true;

        protected override ValueType ValueType { get; } = ValueType.Error;

        public override string AsError() => _error;

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        public override JToken ToJToken()
        {
            var result = new JObject();
            result["$type"] = "error";
            var value = new JObject();
            value["message"] = _error;
            result["value"] = value;
            return result;
        }

        public override string ToString()
        {
            return _error;
        }
    }
}