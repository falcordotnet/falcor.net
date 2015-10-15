using System;

namespace Falcor.Server
{
    public class NoMatch<TValue> : IMatching<TValue>
    {

        private static readonly Lazy<NoMatch<TValue>> LazyInstance = new Lazy<NoMatch<TValue>>(() => new NoMatch<TValue>());
        public static NoMatch<TOther> InstanceOf<TOther>() => NoMatch<TOther>.LazyInstance.Value;
        public static NoMatch<TValue> Instance => LazyInstance.Value;


        public TValue Value
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public FalcorPath Matched
        {
            get
            {
                throw new InvalidOperationException();
            }
        }
        public FalcorPath Unmatched
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public bool IsMatched => false;
        public IMatching<TValue> Where(Func<TValue, bool> predicate) => InstanceOf<TValue>();

        public IMatching<U> Select<U>(Func<TValue, U> selector) => InstanceOf<U>();

        public IMatching<U> SelectMany<U>(Func<TValue, IOption<U>> selector) => InstanceOf<U>();

        public IMatching<U> AndThen<U>(Func<TValue, PathMatcher<U>> then) => InstanceOf<U>();


        public IMatching<TValue> OrElse<U>(IMatching<TValue> matching)
        {
            if (matching == null) throw new ArgumentNullException(nameof(matching));
            return matching;
        }
    }
}