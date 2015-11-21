using Falcor.Server.Routing;

namespace Falcor.Tests
{
    public static class FalcorTestHelpers
    {
        public static IPathValueBuilder Path(params KeySegment[] keys)
            => new PathValueResultHelper(FalcorPath.Create(keys));

        public static IPathValueBuilder Path(FalcorPath path) => new PathValueResultHelper(path);

    }
}