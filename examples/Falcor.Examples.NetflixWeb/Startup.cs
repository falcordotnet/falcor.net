using System;
using Falcor.Examples.Netflix;
using Falcor.Examples.Netflix.RatingService;
using Falcor.Examples.Netflix.RecommendationService;
using Falcor.Server.Owin;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Falcor.Examples.NetflixWeb.Startup))]

namespace Falcor.Examples.NetflixWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseFalcor("/model.json", config => new NetflixRouter(new FakeRatingService(), new RecommendationService(),  1));
        }
    }
}
