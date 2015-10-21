namespace Falcor.Server.Routing
{
    public interface IResponseSerializer
    {
        string Serialize(FalcorResponse response);
    }
}