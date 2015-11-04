using System.Collections.Generic;

namespace Falcor
{
    public interface IPathCollapser
    {
        IEnumerable<FalcorPath> Collapse(IEnumerable<FalcorPath> paths);
    }

    public class PathCollapser : IPathCollapser
    {
        public IEnumerable<FalcorPath> Collapse(IEnumerable<FalcorPath> paths)
        {
            throw new System.NotImplementedException();
        }
    }
}