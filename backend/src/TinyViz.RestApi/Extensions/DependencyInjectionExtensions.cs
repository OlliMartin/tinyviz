namespace TinyViz.RestApi.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddRestApi(IConfiguration configurationRoot) => serviceCollection.AddControllers().Services;
    }

    extension(WebApplication webApplication)
    {
        public WebApplication MapRestApi()
        {
            webApplication.MapControllers();
            return webApplication;
        }
    }
}
