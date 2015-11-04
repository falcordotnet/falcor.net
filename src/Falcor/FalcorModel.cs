using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class FalcorModel : IEnumerable<PathValue>, IJson
    {
        [JsonProperty("jsong")]
        public FalcorTree Cache { get; }

        public FalcorModel(FalcorTree cache)
        {
            Cache = cache;
        }

        public IEnumerator<PathValue> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(FalcorPath path) => Cache.Contains(path);
        public JToken ToJson()
        {
            throw new NotImplementedException();
        }
    }
}