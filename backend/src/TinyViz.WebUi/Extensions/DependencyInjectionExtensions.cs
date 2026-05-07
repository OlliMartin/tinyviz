namespace TinyViz.WebUi.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddRestApi(IConfiguration configurationRoot) => serviceCollection;
    }
}
