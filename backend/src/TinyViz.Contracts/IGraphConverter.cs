using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Contracts;

public interface IGraphConverter
{
    Task<IGraphDescriptor> ConvertAsync(IGraphDescriptor graphDescriptor, CancellationToken cancellationToken = default);
}
