using System;

namespace Falcor.Server
{
    public abstract class Matching<TKey>
    {
        public abstract TKey Value { get; }
        public abstract FalcorPath Matched { get; }
        public abstract FalcorPath Unmatched { get; }
        public abstract bool IsMatched { get; }
        public abstract Matching<TKey> Where(Func<TKey, bool> predicate);
        public abstract Matching<TValue> Select<TValue>(Func<TKey, TValue> selector);
        public abstract Matching<TValue> SelectMany<TValue>(Func<TKey, IOption<TValue>> selector);
        public abstract Matching<TValue> AndThen<TValue>(Func<TKey, PathMatcher<TValue>> then);
        public abstract Matching<TKey> OrElse<U>(Matching<TKey> matching);
    }
}