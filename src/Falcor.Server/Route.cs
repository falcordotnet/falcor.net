using System;

namespace Falcor.Server
{
    public delegate IObservable<RouteResult> Route<TRequest>(RequestContext<TRequest> context);
}
