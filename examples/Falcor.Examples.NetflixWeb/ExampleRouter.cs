using System.Linq;
using System.Threading.Tasks;
using Falcor.Server;

namespace Falcor.Examples.NetflixWeb
{
    public class ExampleRouter : FalcorRouter
    {
        public ExampleRouter()
        {
            Get["foo"] = parameters => Task.FromResult(Complete(new PathValue(FalcorPath.From("foo"), "bar")));
            Get["foo[{ranges:ids}].name"] = parameters =>
            {
                var ids = (NumberRange)parameters.Ids;
                var results = ids.Select(id => new PathValue(FalcorPath.From("foo", id, "name"), "Jill"));
                return Task.FromResult(Complete(results));
            };
        }
    }
}