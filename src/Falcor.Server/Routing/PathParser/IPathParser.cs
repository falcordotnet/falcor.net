using System.Collections.Generic;

namespace Falcor.Server.Routing.PathParser
{
    public interface IPathParser
    {
        IReadOnlyList<FalcorPath> ParseMany(string paths);
        FalcorPath ParseSingle(string path);
    }
}