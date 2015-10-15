using System;

namespace Falcor.Server
{
    public static class PathMatcherExtensions
    {
        public static PathMatcher<TRequest> Where<TRequest>(this PathMatcher<TRequest> matcher, Func<TRequest, bool> predicate) =>
            (matched, unmatched) => matcher(matched, unmatched).Where(predicate);

        public static PathMatcher<TRequest> Select<TRequest>(this PathMatcher<TRequest> matcher, Func<TRequest, TRequest> selector) =>
            (matched, unmatched) => matcher(matched, unmatched).Select(selector);

        public static PathMatcher<TRequest> SelectMany<TRequest>(this PathMatcher<TRequest> matcher, Func<TRequest, IOption<TRequest>> selector) =>
            (matched, unmatched) => matcher(matched, unmatched).SelectMany(selector);
    }
}