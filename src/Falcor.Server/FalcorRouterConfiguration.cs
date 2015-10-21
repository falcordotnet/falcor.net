using System;
using Falcor.Server.Routing;
using Falcor.Server.Routing.PathParser;

namespace Falcor.Server
{
    public sealed class FalcorRouterConfiguration
    {

        public FalcorRouterConfiguration(string path, Func<FalcorRouterConfiguration, FalcorRouter> routerFactory, IServiceProvider serviceProvider = null)
        {
            Path = path;

            if (serviceProvider != null)
            {
                ServiceProvider = serviceProvider;
                RouteParser = ServiceProvider.GetService(typeof(IRouteParser)) as IRouteParser ?? RouteParser;
                PathParser = ServiceProvider.GetService(typeof(IPathParser)) as IPathParser ?? PathParser;
                ResponseSerializer = ServiceProvider.GetService(typeof(IResponseSerializer)) as IResponseSerializer ?? ResponseSerializer;
            }

            RouterFactory = routerFactory;
        }

        public string Path { get; }
        public IServiceProvider ServiceProvider { get; }
        public static IRouteParser RouteParser { get; private set; } = new MemoizedRouteParser(new SpracheRouteParser());
        public static IPathParser PathParser { get; private set; } = new SprachePathParser();
        public static IPathParser MemoizedPathParser { get; private set; } = new MemoizedPathParser(PathParser);
        public static IResponseSerializer ResponseSerializer { get; private set; } = new ResponseSerializer();

        public Func<FalcorRouterConfiguration, FalcorRouter> RouterFactory { get; }
        public FalcorRouter Router => RouterFactory(this);


    }
}