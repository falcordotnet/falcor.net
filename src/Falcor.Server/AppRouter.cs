namespace Falcor.Server
{
    public class AppRouter : FalcorRouter<HttpRequest>
    {
        public AppRouter()

        {
            Get["howdy.hello"] = _ =>
            {
                return null;
            };
        }
    }
}