using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Falcor.Server.Routing.PathParser
{
    internal sealed class SprachePathParser : IPathParser
    {
        public static List<FalcorPath> Empty { get; } = new List<FalcorPath>();


        public IReadOnlyList<FalcorPath> ParseMany(string paths) =>
            string.IsNullOrWhiteSpace(paths) ? Empty : PathGrammar.Paths.Parse(paths).ToList();

        public FalcorPath ParseSingle(string path) =>
            string.IsNullOrWhiteSpace(path) ? FalcorPath.Empty : PathGrammar.Path.Parse(path);
    }
}