using System;

namespace Falcor.Server
{
    public class Match<TValue> : IMatching<TValue>
    {
        public Match(FalcorPath matched, FalcorPath unmatched, TValue value)
        {
            Matched = matched;
            Unmatched = unmatched;
            Value = value;
        }

        public TValue Value { get; }
        public FalcorPath Matched { get; }
        public FalcorPath Unmatched { get; }
        public bool IsMatched => true;
        public IMatching<TValue> Where(Func<TValue, bool> predicate) => predicate(Value) ? (IMatching<TValue>)this : NoMatch<TValue>.InstanceOf<TValue>();

        public IMatching<U> Select<U>(Func<TValue, U> selector) => new Match<U>(Matched, Unmatched, selector(Value));

        public IMatching<U> SelectMany<U>(Func<TValue, IOption<U>> selector)
        {
            var result = selector(Value);
            if (result.IsDefined)
                return new Match<U>(Matched, Unmatched, result.Get());
            return NoMatch<U>.Instance;
        }

        public IMatching<U> AndThen<U>(Func<TValue, PathMatcher<U>> then) => then(Value)(Matched, Unmatched);

        public IMatching<TValue> OrElse<U>(IMatching<TValue> matching) => this;
    }
}