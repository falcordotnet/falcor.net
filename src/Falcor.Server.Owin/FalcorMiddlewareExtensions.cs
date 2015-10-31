using System;
using Falcor.Server.Routing;
using Owin;

namespace Falcor.Server.Owin
{
    public static class FalcorMiddlewareExtensions
    {
        public static void UseFalcor(this IAppBuilder appBuilder, string path, Func<FalcorRouterConfiguration, FalcorRouter> routerFactory, IServiceProvider serviceProvider = null, Action<IAppBuilder> appBuilderConfiguration = null)
        {
            var config = new FalcorRouterConfiguration(path, routerFactory, serviceProvider);
            appBuilder.UseFalcor(config, appBuilderConfiguration);
        }

        public static void UseFalcor(this IAppBuilder appBuilder, FalcorRouterConfiguration config, Action<IAppBuilder> appBuilderConfiguration)
        {
            appBuilder.Map(config.Path, app =>
            {
                app.Use<FalcorMiddleware>(config);
                appBuilderConfiguration?.Invoke(app);
            });
        }
    }
}
