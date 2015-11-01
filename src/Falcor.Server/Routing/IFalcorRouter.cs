using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public interface IFalcorRouter
    {
        RouteCollection Routes { get; }
        Task<FalcorResponse> RouteAsync(FalcorRequest request);
    }
}