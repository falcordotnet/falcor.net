using System;

namespace Falcor.Server
{
    public class Match<TValue> : Matching<TValue>
    {
        public Match(FalcorPath matched, FalcorPath unmatched, TValue value)
        {
            Matched = matched;
            Unmatched = unmatched;
            Value = value;
        }

        public override TValue Value { get; }
        public override FalcorPath Matched { get; }
        public override FalcorPath Unmatched { get; }
        public override bool IsMatched => true;
        public override Matching<TValue> Where(Func<TValue, bool> predicate) => predicate(Value) ? (Matching<TValue>)this : NoMatch<TValue>.InstanceOf<TValue>();

        public override Matching<U> Select<U>(Func<TValue, U> selector) => new Match<U>(Matched, Unmatched, selector(Value));

        public override Matching<U> SelectMany<U>(Func<TValue, IOption<U>> selector)
        {
            var result = selector(Value);
            if (result.IsDefined)
                return new Match<U>(Matched, Unmatched, result.Get());
            return NoMatch<U>.Instance;
        }

        public override Matching<U> AndThen<U>(Func<TValue, PathMatcher<U>> then) => then(Value)(Matched, Unmatched);

        public override Matching<TValue> OrElse<U>(Matching<TValue> matching) => this;
    }
}