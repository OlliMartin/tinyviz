using System.Diagnostics.CodeAnalysis;

namespace Integration.TinyViz.Templating.Framework;

public sealed class VerifyDanglingSnapshots : IAsyncLifetime
{
    [Experimental("VerifyDanglingSnapshots")]
    public ValueTask DisposeAsync()
    {
        DanglingSnapshots.Run();
        return ValueTask.CompletedTask;
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;
}
