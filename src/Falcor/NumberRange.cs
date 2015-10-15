using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Falcor
{
    public sealed class NumberRange : NumericKey, IEquatable<NumberRange>, IEnumerable<long>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NumberRange && Equals((NumberRange)obj);
        }

        private IEnumerable<long> AsEnumerable()
        {
            for (var i = From; i <= To; i++)
                yield return i;
        }
        public override KeyType KeyType { get; } = KeyType.Range;
        public long From { get; }

        /// <summary>
        /// To value of the range
        /// </summary>
        public long To { get; }

        public NumberRange(long from, long to, bool inclusive = true)
        {
            if (inclusive)
                Debug.Assert(to >= from, $"{nameof(to)} >= {nameof(from)}");
            else
                Debug.Assert(to > from, $"{nameof(to)} > {nameof(from)}");

            From = from;
            To = inclusive ? to : (to - 1);
        }

        public NumberRange(long value) : this(value, value) { }

        //public override Range AsRange() => _keys;
        //public override IRangeSet AsNumericSet() => new IRangeSet(_keys);
        public override SortedSet<long> AsSortedNumberSet() => new SortedSet<long>(AsEnumerable());

        public static bool operator ==(NumberRange lhs, NumberRange rhs) => Util.IfBothNullOrEquals(lhs, rhs);

        public static bool operator !=(NumberRange lhs, NumberRange rhs) => !(lhs == rhs);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)KeyType;
                hashCode = (hashCode * 397) ^ From.GetHashCode();
                hashCode = (hashCode * 397) ^ To.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(NumberRange other)
        {
            if (other == null) return false;
            return To == other.To && From == other.From;
        }

        public IEnumerator<long> GetEnumerator() => AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}