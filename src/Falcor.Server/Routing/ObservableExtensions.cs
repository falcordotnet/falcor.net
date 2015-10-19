using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Falcor.Server.Routing
{
    static class ObservableExtensions
    {
        public static IObservable<T> SwitchIfEmpty<T>(this IObservable<T> first, IObservable<T> second)
        {
            var signal = new AsyncSubject<Unit>();
            var source1 = first.Do(item => { signal.OnNext(Unit.Default); signal.OnCompleted(); });
            var source2 = second.TakeUntil(signal);

            return source1.Concat(source2); // if source2 is cold, it won't invoke it until source1 is completed
        }
    }
}
