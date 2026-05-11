using Plotly.NET;
using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Renderer;

public interface IChartBuilder<in TGraphDescriptor, TGraphType> : IChartBuilder
    where TGraphDescriptor : IGraphDescriptor<TGraphType>
{
    bool IChartBuilder.Handles(IGraphDescriptor graphDescriptor) => graphDescriptor is TGraphDescriptor;

    Task<GenericChart> IChartBuilder.BuildAsync(IGraphDescriptor graphDescriptor, double? value, CancellationToken cancellationToken)
    {
        if (graphDescriptor is not TGraphDescriptor typedGraphDescriptor)
        {
            throw new InvalidOperationException(""); // TODO
        }

        return BuildAsync(typedGraphDescriptor, value, cancellationToken);
    }

    Task<GenericChart> BuildAsync(TGraphDescriptor graphDescriptor, double? value, CancellationToken cancellationToken = default);
}
