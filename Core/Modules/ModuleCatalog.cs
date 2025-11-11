using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Holds the list of startup modules and coordinates their execution order.
    /// </summary>
    public sealed class ModuleCatalog
    {
        private readonly IReadOnlyList<IStartupModule> _modules;

        public ModuleCatalog(IEnumerable<IStartupModule> modules)
        {
            _modules = modules.ToList();
        }

        public static ModuleCatalog CreateDefault() => new(new IStartupModule[]
        {
            new ConfigurationModule(),
            new LoggingModule(),
            new MvcAndCachingModule(),
            new DependencyInjectionModule(),
            new PollyModule(),
            new HttpClientModule(),
            new MiddlewareModule(),
            new RoutingModule(),
            new StartupValidationModule()
        });

        public void ConfigureServices(WebApplicationBuilder builder)
        {
            foreach (var module in _modules)
            {
                module.ConfigureServices(builder);
            }
        }

        public void ConfigureApplication(WebApplication app)
        {
            foreach (var module in _modules)
            {
                module.ConfigureApplication(app);
            }
        }
    }
}
