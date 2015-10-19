using System;

namespace Falcor.Server.Routing
{
    public delegate IObservable<RouteResult> Route(RequestContext context);
}
