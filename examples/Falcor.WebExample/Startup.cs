using System;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Owin;
using Falcor.WebExample;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Falcor.WebExample
{
    public class ExampleRouter : FalcorRouter
    {
        public ExampleRouter()
        {
            //Get["howdy.hello"] = _ => Task.FromResult(Complete(null));
            //Call["howdy.hello.watchaDoin"] = _ => Task.FromResult(Complete(null));
            /*
            Get["foo"] = async parameters =>
            {
                await Task.Delay(1000);
                return Complete(new PathValue(FalcorPath.From("foo"), "bar"));
            };
            */
        }
    }


    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var serviceProvider = new TempServiceProvider();
            //app.UseFalcor("/model.json", serviceProvider, sp => new ExampleRouter());
            app.Map("", a =>
            {
                a.Use<FalcorMiddleware>(new FalcorConfiguration("/model.json", serviceProvider,
                    sp => new ExampleRouter()));
                a.UseErrorPage(new ErrorPageOptions() { ShowEnvironment = true });
            });
            app.UseErrorPage(new ErrorPageOptions()
            {
                ShowEnvironment = true
            });

            app.Run(async context =>
            {
                throw new Exception("UseErrorPage() demo");
                await context.Response.WriteAsync("Error page demo");
            });

            //app.UseCors(CorsOptions.AllowAll);


            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("howdy");
            //});
        }
    }

    public class TempServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
