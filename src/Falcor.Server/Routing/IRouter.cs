using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public interface IRouter
    {
        RouteCollection Routes { get; }
        Task<FalcorResponse> RouteAsync(FalcorRequest request);
    }
}