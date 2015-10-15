using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Falcor
{
    public class FalcorPath : IEnumerable<KeySegment>, IEquatable<FalcorPath>
    {
        public static FalcorPath Empty { get; } = new FalcorPath(new List<KeySegment>());

        private readonly KeySegment[] _keys;

        protected FalcorPath(IEnumerable<KeySegment> keys)
        {
            _keys = keys.ToArray();
            var test = _keys.GetEnumerator();
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

        //public FalcorPath Prepend(KeySegment key)
        //{
        //    throw new NotImplementedException();
        //}

        //public FalcorPath PrependAll(FalcorPath path)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerator<KeySegment> GetEnumerator()
        {
            return (IEnumerator<KeySegment>)_keys.GetEnumerator();
        }

        public override bool Equals(object obj) => Equals((FalcorPath)obj);
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(FalcorPath lhs, FalcorPath rhs) => Util.IfBothNullOrEquals(lhs, rhs);

        public static bool operator !=(FalcorPath lhs, FalcorPath rhs) => !(lhs == rhs);

        public KeySegment this[int i] => _keys[i];

    }
}