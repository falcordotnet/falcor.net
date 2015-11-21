using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Routing;

namespace Falcor.Tests.Server.Routing
{
    public class TestFalcorRouter : FalcorRouter
    {
        static List<TodoItem> todos = new List<TodoItem>
        {
            new TodoItem
            {
                name = "get milk from corner store",
                done = false
            },
            new TodoItem
            {
                name= "froth milk",
                done= true
            },
            new TodoItem
            {
                name= "make coffee",
                done= false
            }
        };

        public TestFalcorRouter()
        {
            Get["foo[{integers:ids}].name"] = parameters =>
            {
                NumericSet ids = parameters.ids;
                var results = ids.Select(id => new PathValue(FalcorPath.Create("foo", id, "name"), "Jill-" + id));
                return CompleteAsync(results);
            };

            Get["foo"] = parameters => CompleteAsync(new PathValue(FalcorPath.Create("foo"), "bar"));

            Set["todos[{integers:ids}].done"] = async parameters =>
            {
                try
                {
                    NumericSet ids = parameters.ids;
                    dynamic jsonGraph = parameters.jsonGraph;

                    ids.ToList().ForEach(id =>
                    {
                        todos[id].done = (bool)jsonGraph["todos"][id.ToString()]["done"];
                    });

                    var result = await Task.FromResult(ids.Select(id => Path("todos", id).Key("done").Atom(todos[id].done)));
                    return Complete(result);
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }

        // Test helper methods
        public static Task<RouteHandlerResult> CompleteAsync(params PathValue[] values) => CompleteAsync(values.ToList());

        public static Task<RouteHandlerResult> CompleteAsync(IEnumerable<PathValue> values)
            => Task.FromResult(FalcorRouter.Complete(values.ToList()));
    }

    internal class TodoItem
    {
        public string name { get; set; }
        public bool done { get; set; }
    }
}