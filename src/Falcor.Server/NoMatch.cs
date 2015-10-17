using System;

namespace Falcor.Server
{
    public class NoMatch<TValue> : Matching<TValue>
    {
        private static readonly Lazy<NoMatch<TValue>> LazyInstance = new Lazy<NoMatch<TValue>>(() => new NoMatch<TValue>());
        public static NoMatch<TOther> InstanceOf<TOther>() => NoMatch<TOther>.LazyInstance.Value;
        public static NoMatch<TValue> Instance => LazyInstance.Value;


        public override TValue Value
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override FalcorPath Matched
        {
            get
            {
                throw new InvalidOperationException();
            }
        }
        public override FalcorPath Unmatched
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override bool IsMatched => false;
        public override Matching<TValue> Where(Func<TValue, bool> predicate) => InstanceOf<TValue>();

        public override Matching<U> Select<U>(Func<TValue, U> selector) => InstanceOf<U>();

        public override Matching<U> SelectMany<U>(Func<TValue, IOption<U>> selector) => InstanceOf<U>();

        public override Matching<U> AndThen<U>(Func<TValue, PathMatcher<U>> then) => InstanceOf<U>();

        public override Matching<TValue> OrElse<U>(Matching<TValue> matching)
        {
            if (matching == null) throw new ArgumentNullException(nameof(matching));
            return matching;
        }
    }
}