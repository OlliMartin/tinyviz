using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TinyViz.WebUi.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddWebUi(IConfiguration configurationRoot) =>
            serviceCollection
                .AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddCircuitOptions(options =>
                {
                    options.DetailedErrors = true;
                })
                .Services;
    }

    extension(WebApplication app)
    {
        public WebApplication MapWebUi()
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
            }

            app.UseAntiforgery();
            app.MapStaticAssets();

            app.MapRazorComponents<WebUi>().AddInteractiveServerRenderMode();

            return app;
        }
    }
}
