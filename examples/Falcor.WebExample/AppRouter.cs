using System.Threading.Tasks;
using Falcor.Server;

namespace Falcor.WebExample
{
    public class AppRouter : FalcorRouter
    {
        public AppRouter()
        {
            //Get["howdy.hello"] = _ => Task.FromResult(Complete(null));
            //Call["howdy.hello.watchaDoin"] = _ => Task.FromResult(Complete(null));
            Set["cool.huh"] = async parameters =>
            {
                await Task.Delay(1000);
                return null;
            };
        }
    }
}