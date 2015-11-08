using Falcor.Server.Routing.PathParser;

namespace Falcor.Tests.Server.Routing
{
    public class SpracheParserTests : ParserTests
    {
        public override IPathParser Parser { get; } = new SprachePathParser();
    }
}