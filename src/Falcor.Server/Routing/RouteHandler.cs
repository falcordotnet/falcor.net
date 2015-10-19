using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public delegate Task<RouteHandlerResult> RouteHandler(dynamic parameters);
}