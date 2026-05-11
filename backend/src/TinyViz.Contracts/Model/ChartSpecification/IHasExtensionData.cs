namespace TinyViz.Contracts.Model.ChartSpecification;

public interface IHasExtensionData<out TSelf>
    where TSelf : IHasExtensionData<TSelf>
{
    Dictionary<string, object?> ExtensionData { get; }

    TSelf WithExtensionData(Dictionary<string, object?> extensionData);

    Dictionary<string, object?> GetPrimitiveExtensionData();

    TSelf ToPrimitiveExtensionData()
    {
        var result = new Dictionary<string, object?>(ExtensionData.Count);

        foreach (var (key, value) in ExtensionData)
        {
            result[key] = JsonHelper.UnwrapValue(value);
        }

        return WithExtensionData(result);
    }
}
