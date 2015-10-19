using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace Falcor
{
    public class FalcorPath : IEquatable<FalcorPath>, IReadOnlyList<KeySegment>
    {
        public static FalcorPath Empty { get; } = new FalcorPath(new List<KeySegment>());

        private readonly KeySegment[] _keys;
        private List<KeySegment> KeysList => _keys.ToList();

        public FalcorPath(params KeySegment[] keys)
        {
            _keys = keys;
        }

        public FalcorPath(IEnumerable<KeySegment> keys) : this(keys.ToArray())
        {
        }

        public FalcorPath Append(IEnumerable<KeySegment> keys) =>
            From(_keys.Concat(keys));


        public bool Equals(FalcorPath other)
        {
            var isMatch = _keys.SequenceEqual(other._keys);
            return isMatch;
        }

        public static FalcorPath From(IEnumerable<KeySegment> output) =>

        new FalcorPath(output.ToList());
        public static FalcorPath From(params KeySegment[] keys) =>
            From(keys.ToList());


        public FalcorPath Append(KeySegment key)
        {
            var result = new KeySegment[_keys.Length + 1];
            _keys.CopyTo(result, 0);
            result[_keys.Length] = key;
            return new FalcorPath(result);
        }

        public FalcorPath AppendAll(FalcorPath path)
        {
            var result = new KeySegment[_keys.Length + path.Count()];
            _keys.CopyTo(result, 0);
            path._keys.CopyTo(result, _keys.Length);
            return new FalcorPath(result);
        }

        public KeySegment Head => _keys.First();
        public FalcorPath Tail => new FalcorPath(_keys.Skip(1));

        //public FalcorPath Prepend(KeySegment key)
        //{
        //    throw new NotImplementedException();
        //}

        //public FalcorPath PrependAll(FalcorPath path)
        //{
        //    throw new NotImplementedException();
        //}


        [DebuggerStepThrough]
        public IEnumerator<KeySegment> GetEnumerator() => KeysList.GetEnumerator();

        public override bool Equals(object obj) => Equals((FalcorPath)obj);

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool operator ==(FalcorPath lhs, FalcorPath rhs) => Util.IfBothNullOrEquals(lhs, rhs);

        public static bool operator !=(FalcorPath lhs, FalcorPath rhs) => !(lhs == rhs);

        public static implicit operator FalcorPath(List<KeySegment> keys) => new FalcorPath(keys);

        public KeySegment this[int i] => _keys[i];

        public int Count => _keys.Length;
    }
}