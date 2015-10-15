using System;
using System.Collections.Generic;
using System.Linq;

namespace Falcor
{
    internal static class Util
    {
        public static void ThrowIfArgumentNull(object value, string paramName)
        {
            if (value == null) throw new ArgumentNullException(paramName);
        }

        public static bool IfBothNullOrEquals<T, T2>(T lhs, T2 rhs) where T : IEquatable<T2>
        {
            // Check first to see if left and right hand side is null, otherwise check for equality
            var lhsNull = ReferenceEquals(lhs, null);
            if (lhsNull)
            {
                return ReferenceEquals(rhs, null);
            }
            var lhsEqualsRhs = lhs.Equals(rhs);
            return lhsEqualsRhs;
        }

        public static bool Empty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}