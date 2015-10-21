using System;
using Owin;

namespace Falcor.Server.Owin
{
    public static class FalcorMiddlewareExtensions
    {

        public static void UseFalcor(this IAppBuilder appBuilder, string path, IServiceProvider serviceProvider, Func<IServiceProvider, FalcorRouter> routerFactory)
        {
            var config = new FalcorConfiguration(path, serviceProvider, routerFactory);
            appBuilder.UseFalcor(config);
        }

        public static void UseFalcor(this IAppBuilder appBuilder, FalcorConfiguration config)
        {
            appBuilder.Map(config.Path, app => app.Use<FalcorMiddleware>(config));
        }
    }
}
