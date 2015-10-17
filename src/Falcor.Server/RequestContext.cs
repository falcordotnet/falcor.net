using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Falcor.Server.Reactive;

namespace Falcor.Server
{
    public class RequestContext<TRequest>
    {
        public FalcorPath Matched { get; }
        public FalcorPath Unmatched { get; }
        public FalcorRequest<TRequest> Request { get; }

        public RequestContext(FalcorRequest<TRequest> request, FalcorPath matched, FalcorPath unmatched)
        {
            Request = request;
            Matched = matched;
            Unmatched = unmatched;
        }

        public RequestContext<TRequest> WithRequest(FalcorRequest<TRequest> request) => new RequestContext<TRequest>(request, Matched, Unmatched);
        public RequestContext<TRequest> WithPaths(FalcorPath matched, FalcorPath unmatched) => new RequestContext<TRequest>(Request, matched, unmatched);

        public static IObservable<RouteResult> Reject(string error) => Observable.Return(RouteResult.Reject(error));
        public static IObservable<RouteResult> Reject() => Observable.Return(RouteResult.Reject());
        public static IObservable<RouteResult> Complete(FalcorPath matched, FalcorPath unmatched, FalcorValue value) => Observable.Return(RouteResult.Complete(unmatched, new PathValue(matched, value)));
        public IObservable<RouteResult> Complete(FalcorValue value) => Complete(Matched, Unmatched, value);

        private IObservable<RouteResult> FirstOf(IReadOnlyList<Route<TRequest>> routes, int index)
        {
            if (routes.Count > index)
            {
                return routes[index](this)
                    .Where(r => r.IsComplete)
                    .SwitchIfEmpty(Observable.Defer(() => FirstOf(routes, index++)));
            }

            return Reject();
        }

        public IObservable<RouteResult> FirstOf(IReadOnlyList<Route<TRequest>> routes) => FirstOf(routes, 0);
    }
}