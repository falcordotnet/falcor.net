using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class Ref : FalcorValue, IEnumerable<KeySegment>
    {
        private readonly FalcorPath _path;

        public Ref(FalcorPath path)
        {
            _path = path;
        }

        public override bool IsValue => false;

        protected override ValueType ValueType { get; } = ValueType.Ref;

        [DebuggerStepThrough]
        public IEnumerator<KeySegment> GetEnumerator() => _path.GetEnumerator();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override JToken ToJson() => new JObject
        {
            ["$type"] = "ref",
            ["value"] = _path.ToJson()
        };

        public override FalcorPath AsRef() => _path;
    }
}