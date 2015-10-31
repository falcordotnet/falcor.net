using System.Threading.Tasks;
using Falcor.Examples.Netflix.RatingService;
using Falcor.Examples.Netflix.RecommendationService;
using Falcor.Examples.Netflix.Web;
using Falcor.Server;
using Falcor.Server.Owin;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Falcor.Examples.Netflix.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseFalcor("/model.json",
                routerFactory: config => 
                    new NetflixRouter(new FakeRatingService(), 
                                      new FakeRecommendationService(), userId: 1));

            app.UseStaticFiles();
        }
    }


}
