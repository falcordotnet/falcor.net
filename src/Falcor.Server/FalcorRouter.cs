using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Falcor.Server.Routing;

namespace Falcor.Server
{
    public abstract class FalcorRouter
    {
        public FalcorRouter()
        {
            _route = Routes.FirstToComplete();
        }

        public List<Route> Routes { get; } = new List<Route>();
        private readonly FalcorModel _model = new FalcorModel();
        private readonly Route _route;

        protected RouteBuilder Get => new RouteBuilder(FalcorMethod.Get, this);
        protected RouteBuilder Set => new RouteBuilder(FalcorMethod.Set, this);
        protected RouteBuilder Call => new RouteBuilder(FalcorMethod.Call, this);

        protected RouteHandlerResult Complete(List<PathValue> values) => new CompleteHandlerResult(values);
        protected RouteHandlerResult Error(string error = null) => new ErrorHandlerResult(error);

        private IObservable<PathValue> Resolve(Route route, RequestContext context)
        {
            if (!context.Unmatched.Any() || _model.Contains(context.Unmatched))
                return Observable.Empty<PathValue>();

            var results = route(context).SelectMany(result =>
            {
                if (result.IsComplete)
                {
                    var pathValues = result.Values;
                    if (pathValues.Any())
                    {
                        //TODO: this.model.putAll(pathValues);
                        if (result.UnmatchedPath.Any())
                        {
                            return pathValues.ToObservable()
                                .Where(pathValue => pathValue.Value.IsRef)
                                .SelectMany(pathValue =>
                                {
                                    var unmatched = pathValue.Value.AsRef().AppendAll(result.UnmatchedPath);
                                    return Resolve(route, context.WithUnmatched(unmatched));
                                })
                                //.StartWith(pathValues) // Is this nescessary?
                                ;
                        }
                    }
                }
                else
                {
                    var error = new Error(result.Error);
                    //TODO: this.model.put(context.getUnmatched(), (FalcorValue)error);
                    return Observable.Return(new PathValue(context.Unmatched, error));
                }
                return Observable.Empty<PathValue>();
            });

            return results;
        }

        public IObservable<PathValue> Route(FalcorRequest request) =>
            request.Paths.ToObservable().SelectMany(unmatched => Resolve(_route, new RequestContext(request, unmatched)));

    }
}