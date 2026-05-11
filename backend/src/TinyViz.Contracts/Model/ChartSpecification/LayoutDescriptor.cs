using System.Text.Json.Serialization;
using TinyViz.Contracts.Model.Comparer;

namespace TinyViz.Contracts.Model.ChartSpecification;

[UsedImplicitly]
public record LayoutDescriptor : IHasExtensionData<LayoutDescriptor>
{
    public static IEqualityComparer<LayoutDescriptor> JsonComparer { get; } = new LayoutDescriptorEqualityComparer();

    [JsonExtensionData]
    public Dictionary<string, object?> ExtensionData { get; init; } = new();

    public Dictionary<string, object?> GetPrimitiveExtensionData() =>
        ((IHasExtensionData<LayoutDescriptor>)this).ToPrimitiveExtensionData().ExtensionData;

    public LayoutDescriptor WithExtensionData(Dictionary<string, object?> extensionData) => this with { ExtensionData = extensionData };

    private class LayoutDescriptorEqualityComparer : ExtensionDataComparerBase<LayoutDescriptor>
    {
        protected override bool PropertiesEqual(LayoutDescriptor x, LayoutDescriptor y) => true;

        protected override int GetPropertiesHashCode(LayoutDescriptor obj)
        {
            HashCode hashCode = default;
            return hashCode.ToHashCode();
        }
    }
}
