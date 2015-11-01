using System;
using System.Collections;
using System.Collections.Generic;

namespace Falcor
{
    public sealed class FalcorModel : IEnumerable<PathValue>
    {
        private readonly FalcorTree _cache;

        public FalcorModel(FalcorTree cache)
        {
            _cache = cache;
        }

        public IEnumerator<PathValue> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(FalcorPath path) => _cache.Contains(path);
    }
}