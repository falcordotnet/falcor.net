using System;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class Atom : FalcorValue
    {
        public Atom(object value, TimeSpan? expires = null)
        {
            Value = value;
            if (expires != null)
                Expires = Convert.ToInt64(expires.Value.TotalMilliseconds);
        }

        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public object Value { get; }

        public override bool IsValue => true;
        public long? Expires { get; }

        protected override ValueType ValueType { get; } = ValueType.Atom;

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        public override JToken ToJToken()
        {
            var result = new JObject {["$type"] = "atom"};
            result["$timestamp"] = Timestamp.Ticks;
            if (Expires.HasValue) result["$expires"] = Expires;
            var value = SerializationHelper.SerializeItem(Value);
            result["value"] = value;
            return result;
        }
    }
}