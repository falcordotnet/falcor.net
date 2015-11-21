using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public class FalcorPath : IEquatable<FalcorPath>, IReadOnlyList<KeySegment>, IJson
    {
        public bool Equals(FalcorPath other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _keys.SequenceEqual(other._keys);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is FalcorPath && Equals((FalcorPath)obj);
        }

        public override int GetHashCode() => _keys?.GetHashCode() ?? 0;

        private readonly KeySegment[] _keys;

        private FalcorPath(params KeySegment[] keys)
        {
            _keys = keys;
        }

        private FalcorPath(IEnumerable<KeySegment> keys) : this(keys.ToArray())
        {
        }

        public static FalcorPath Empty { get; } = new FalcorPath(new List<KeySegment>());
        private List<KeySegment> KeysList => _keys.ToList();

        public KeySegment Head => _keys.First();
        public FalcorPath Tail => new FalcorPath(_keys.Skip(1));

        [DebuggerStepThrough]
        public IEnumerator<KeySegment> GetEnumerator() => KeysList.GetEnumerator();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public KeySegment this[int i] => _keys[i];

        public int Count => _keys.Length;

        public FalcorPath Append(IEnumerable<KeySegment> keys) =>
            Create(_keys.Concat(keys));

        public static FalcorPath Create(IEnumerable<KeySegment> output) =>
            new FalcorPath(output.ToList());

        public static FalcorPath Create(params KeySegment[] keys) => Create(keys.ToList());


        public FalcorPath Append(KeySegment key)
        {
            var result = new KeySegment[_keys.Length + 1];
            _keys.CopyTo(result, 0);
            result[_keys.Length] = key;
            return new FalcorPath(result);
        }


        public FalcorPath AppendAll(FalcorPath path)
        {
            var result = new KeySegment[_keys.Length + path.Count];
            _keys.CopyTo(result, 0);
            path._keys.CopyTo(result, _keys.Length);
            return new FalcorPath(result);
        }

        public JToken ToJson() => new JArray(_keys.Select(key => key.ToJson()));

        public static implicit operator FalcorPath(List<KeySegment> keys) => new FalcorPath(keys);
    }
}