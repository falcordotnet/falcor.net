using System;

namespace Falcor.Server
{
    public interface IMatching<TValue>
    {
        TValue Value { get; }
        FalcorPath Matched { get; }
        FalcorPath Unmatched { get; }
        bool IsMatched { get; }
        IMatching<TValue> Where(Func<TValue, bool> predicate);
        IMatching<U> Select<U>(Func<TValue, U> selector);
        IMatching<U> SelectMany<U>(Func<TValue, IOption<U>> selector);
        IMatching<U> AndThen<U>(Func<TValue, PathMatcher<U>> then);
        IMatching<TValue> OrElse<U>(IMatching<TValue> matching);
    }
}