using Microsoft.AspNetCore.Builder;

namespace TESTPROJESI.Core.Modules
{
    /// <summary>
    /// Represents a modular startup component that can participate in
    /// configuring the <see cref="WebApplicationBuilder"/> and the resulting
    /// <see cref="WebApplication"/> instance.
    /// </summary>
    public interface IStartupModule
    {
        /// <summary>
        /// Registers services and performs configuration that must run before the
        /// application is built.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        void ConfigureServices(WebApplicationBuilder builder);

        /// <summary>
        /// Customises the HTTP request pipeline or performs any logic that must
        /// execute right after <see cref="WebApplication"/> is created but
        /// before <c>app.Run()</c> is invoked.
        /// </summary>
        /// <param name="app">The constructed web application.</param>
        void ConfigureApplication(WebApplication app);
    }
}
