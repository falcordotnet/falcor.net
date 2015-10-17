using System;

namespace Falcor.Server
{
    public static class PathMatcherExtensions
    {
        public static PathMatcher<TKey> Where<TKey>(this PathMatcher<TKey> matcher, Func<TKey, bool> predicate) =>
            (matched, unmatched) => matcher(matched, unmatched).Where(predicate);

        public static PathMatcher<TKey> Select<TKey>(this PathMatcher<TKey> matcher, Func<TKey, TKey> selector) =>
            (matched, unmatched) => matcher(matched, unmatched).Select(selector);

        public static PathMatcher<TValue> SelectMany<TKey, TValue>(this PathMatcher<TKey> matcher, Func<TKey, IOption<TValue>> selector) =>
            (matched, unmatched) => matcher(matched, unmatched).SelectMany(selector);
    }
}