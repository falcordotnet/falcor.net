using System;
using System.Reactive.Linq;

namespace Falcor.Server.Routing
{
    public sealed class RequestContext
    {
        public RequestContext(FalcorRequest request, FalcorPath unmatched, dynamic parameters = null)
        {
            Request = request;
            Unmatched = unmatched;
            Parameters = parameters;
        }

        public FalcorRequest Request { get; }
        public FalcorPath Unmatched { get; }
        public dynamic Parameters { get; }

        public RequestContext WithUnmatched(FalcorPath unmatched, dynamic parameters = null) =>
            new RequestContext(Request, unmatched, parameters);


        public IObservable<RouteResult> Reject(string error = null) => Observable.Return(RouteResult.Reject(error));

        public IObservable<RouteResult> Complete(FalcorPath unmatched, params PathValue[] values)
            => Observable.Return(RouteResult.Complete(unmatched, values));
    }
}