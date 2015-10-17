using System;
using System.Threading.Tasks;
using Sprache;

namespace Falcor.Server
{
    public class RouteBuilder<TRequest>
    {
        private readonly FalcorRouter<TRequest> _router;
        private readonly FalcorMethod _method;

        public RouteBuilder(FalcorMethod method, FalcorRouter<TRequest> router)
        {
            _method = method;
            _router = router;
        }

        //public Route<TRequest> this[string path]
        //{
        //    set { AddRoute(value, path); }
        //}

        public Func<dynamic, Task<RouteResult>> this[string path]
        {
            set { AddRoute(null, ""); }
        }



        private void AddRoute(Route<TRequest> route, string path)
        {
            //var pathMatchers = RoutingGrammar.Route.Parse(path);
            //_router.Routes.Add(Route.From(_method, handler, pathMatchers));
        }
    }


}