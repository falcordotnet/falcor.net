﻿using System;
using System.Reactive.Linq;
using Falcor.Server.Utils;

namespace Falcor.Server.Routing
{
    public class RequestContext
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
        public IObservable<RouteResult> Compelte(FalcorPath unmatched, params PathValue[] values) => Observable.Return(RouteResult.Complete(unmatched, values));
    }
}