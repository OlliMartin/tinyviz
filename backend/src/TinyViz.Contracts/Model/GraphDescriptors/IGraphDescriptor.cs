namespace TinyViz.Contracts.Model.GraphDescriptors;

public interface IGraphDescriptor
{
    QueryDefinition Query { get; }

    object Untyped { get; }
}
