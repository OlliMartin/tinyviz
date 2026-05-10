using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Contracts;

public interface IGraphConverter<in TIn, TOut> : IGraphConverter
{
    Task<IGraphDescriptor<TOut>> ConvertAsync(IGraphDescriptor<TIn> graphDescriptor, CancellationToken cancellationToken = default);

    async Task<IGraphDescriptor> IGraphConverter.ConvertAsync(IGraphDescriptor graphDescriptor, CancellationToken cancellationToken)
    {
        if (graphDescriptor is not IGraphDescriptor<TIn> typedGraphDescriptor)
        {
            throw new NotSupportedException($"Cannot convert graph descriptor of type {graphDescriptor.GetType().FullName}");
        }

        return await ConvertAsync(typedGraphDescriptor, cancellationToken);
    }
}
