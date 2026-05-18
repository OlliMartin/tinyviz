using Microsoft.Extensions.DependencyInjection;
using TinyViz.Templating;
using TinyViz.Templating.Extensions;

namespace Integration.TinyViz.Templating.Framework;

[UsedImplicitly]
public sealed class TemplatingTestRuntime : IAsyncLifetime
{
    private ServiceProvider? _serviceProvider;

    public ITemplatingEngine TemplatingEngine =>
        _serviceProvider?.GetRequiredService<ITemplatingEngine>()
        ?? throw new InvalidOperationException("No service provider populated. How did that happen?");

    public ValueTask DisposeAsync()
    {
        _serviceProvider?.Dispose();

        return ValueTask.CompletedTask;
    }

    public ValueTask InitializeAsync()
    {
        _serviceProvider = new ServiceCollection().AddTemplatingEngine().BuildServiceProvider();

        return ValueTask.CompletedTask;
    }
}
