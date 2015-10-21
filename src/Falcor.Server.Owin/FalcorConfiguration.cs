using System;

namespace Falcor.Server.Owin
{
    public class FalcorConfiguration
    {
        public FalcorConfiguration(string path, IServiceProvider serviceProvider, Func<IServiceProvider, FalcorRouter> routerFactory)
        {
            Path = path;
            ServiceProvider = serviceProvider;
            RouterFactory = routerFactory;
        }

        public string Path { get; }
        public IServiceProvider ServiceProvider { get; }
        public Func<IServiceProvider, FalcorRouter> RouterFactory { get; }
        public FalcorRouter Router => RouterFactory(ServiceProvider);
    }
}