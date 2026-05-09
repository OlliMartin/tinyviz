using System.Diagnostics;
using Plotly.NET;
using Plotly.NET.ImageExport;

namespace TinyViz.Renderer.Renderers;

public class PngGraphRenderer : IGraphRenderer
{
    public async Task<string> RenderAsync(GenericChart chart, int dimensions, CancellationToken cancellationToken = default)
    {
        const int multiplier = 1;

        // var filePath = Path.GetTempFileName();
        // await chart.SavePNGAsync(filePath, Height: dimensions * multiplier, Width: dimensions * multiplier);
        // Process.Start(new ProcessStartInfo($"{filePath}.png") { UseShellExecute = true });

        return await chart.ToBase64PNGStringAsync(Width: dimensions * multiplier, Height: dimensions * multiplier);
    }
}
