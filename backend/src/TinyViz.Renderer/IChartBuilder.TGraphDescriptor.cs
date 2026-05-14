using Plotly.NET;
using TinyViz.Contracts.Model;
using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Renderer;

public interface IChartBuilder<in TGraphDescriptor, TGraphType> : IChartBuilder
    where TGraphDescriptor : IGraphDescriptor<TGraphType>
{
    bool IChartBuilder.Handles(IGraphDescriptor graphDescriptor) => graphDescriptor is TGraphDescriptor;

    Task<GenericChart> IChartBuilder.BuildAsync(
        IGraphDescriptor graphDescriptor,
        RangeQueryResult rangeQueryResult,
        CancellationToken cancellationToken
    )
    {
        if (graphDescriptor is not TGraphDescriptor typedGraphDescriptor)
        {
            throw new InvalidOperationException(""); // TODO
        }

        return BuildAsync(typedGraphDescriptor, rangeQueryResult, cancellationToken);
    }

    Task<GenericChart> BuildAsync(
        TGraphDescriptor graphDescriptor,
        RangeQueryResult rangeQueryResult,
        CancellationToken cancellationToken = default
    );
}
