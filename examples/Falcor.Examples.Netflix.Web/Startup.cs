using Falcor.Examples.Netflix.RatingService;
using Falcor.Examples.Netflix.RecommendationService;
using Falcor.Examples.Netflix.Web;
using Falcor.Server.Owin;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Falcor.Examples.Netflix.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseFalcor("/model.json", config =>
                new NetflixFalcorRouter(new FakeRatingService(),
                    new FakeRecommendationService(), 1));

            app.UseStaticFiles();
        }
    }
}