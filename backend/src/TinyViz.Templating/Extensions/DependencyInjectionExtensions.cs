using Microsoft.Extensions.DependencyInjection;
using TinyViz.Templating.Internal;
using TinyViz.Templating.Internal.NodeFactories;

namespace TinyViz.Templating.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddTemplatingEngine() =>
            serviceCollection
                .AddSingleton<ITemplatingEngine, DefaultTemplatingEngine>()
                .AddSingleton<INodeFactory, ExtendsNodeFactory>()
                .AddSingleton<INodeFactory, NamedNodeFactory>()
                .AddSingleton<INodeFactory, KeyPrimitiveValueNodeFactory>()
                .AddSingleton<INodeFactory, PrimitiveNodeFactory>();
    }
}
