using System.Text.Json.Serialization;
using TinyViz.Contracts.Model.Comparer;

namespace TinyViz.Contracts.Model.ChartSpecification;

[UsedImplicitly]
public record TraceDescriptor : IHasExtensionData<TraceDescriptor>
{
    [JsonRequired]
    public required string TypeName { get; init; }

    public static IEqualityComparer<TraceDescriptor> JsonComparer { get; } = new TraceDescriptorEqualityComparer();

    [JsonExtensionData]
    public Dictionary<string, object?> ExtensionData { get; init; } = new();

    public TraceDescriptor ToPrimitiveExtensionData()
    {
        var result = new Dictionary<string, object?>(ExtensionData.Count);

        foreach (var (key, value) in ExtensionData)
        {
            result[key] = JsonHelper.UnwrapValue(value);
        }

        return this with
        {
            ExtensionData = result,
        };
    }

    private class TraceDescriptorEqualityComparer : ExtensionDataComparerBase<TraceDescriptor>
    {
        protected override bool PropertiesEqual(TraceDescriptor x, TraceDescriptor y) => x.TypeName == y.TypeName;

        protected override int GetPropertiesHashCode(TraceDescriptor obj)
        {
            HashCode hashCode = default;
            hashCode.Add(obj.TypeName);
            return hashCode.ToHashCode();
        }
    }
}
