namespace Falcor.Server
{
    public interface IRoutePattern : IKeySegment
    {
        bool IsNamed { get; }
        string Name { get; }
    }
}