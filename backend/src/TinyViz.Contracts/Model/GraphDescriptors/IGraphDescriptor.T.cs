namespace TinyViz.Contracts.Model.GraphDescriptors;

public interface IGraphDescriptor<out T> : IGraphDescriptor
{
    T Typed { get; }

    object IGraphDescriptor.Untyped => Typed;
}
