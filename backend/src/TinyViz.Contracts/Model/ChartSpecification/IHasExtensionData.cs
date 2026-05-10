namespace TinyViz.Contracts.Model.ChartSpecification;

public interface IHasExtensionData<out TSelf>
    where TSelf : IHasExtensionData<TSelf>
{
    Dictionary<string, object?> ExtensionData { get; }
    TSelf ToPrimitiveExtensionData();
}
