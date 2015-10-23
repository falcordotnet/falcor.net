using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        public override JToken ToJToken() => new JObject
        {
            ["$type"] = "ref",
            ["value"] = new JArray(_path.Select(k => k.ToJToken()))
        };

        protected override ValueType ValueType { get; } = ValueType.Ref;

        public override FalcorPath AsRef() => _path;

        [DebuggerStepThrough]
        public IEnumerator<KeySegment> GetEnumerator() => _path.GetEnumerator();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}