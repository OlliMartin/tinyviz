using System.Text.Json;
using System.Text.Json.Nodes;

namespace TinyViz.Contracts;

public static class JsonHelper
{
    public static object? UnwrapValue(object? value) =>
        value switch
        {
            JsonElement element => UnwrapElement(element),
            JsonNode node => UnwrapNode(node),
            var _ => value,
        };

    private static object? UnwrapElement(JsonElement element) =>
        element.ValueKind switch
        {
            JsonValueKind.Object => element.EnumerateObject().ToDictionary(p => p.Name, p => UnwrapElement(p.Value)),

            JsonValueKind.Array => element.EnumerateArray().Select(UnwrapElement).ToList(),

            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => UnwrapNumber(element),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            var _ => element.GetRawText(),
        };

    private static object? UnwrapNode(JsonNode? node) =>
        node switch
        {
            JsonObject obj => obj.ToDictionary(p => p.Key, p => UnwrapNode(p.Value)),

            JsonArray arr => arr.Select(UnwrapNode).ToList(),

            JsonValue val when val.TryGetValue<bool>(out var b) => b,
            JsonValue val when val.TryGetValue<long>(out var l) => l,
            JsonValue val when val.TryGetValue<double>(out var d) => d,
            JsonValue val when val.TryGetValue<string>(out var s) => s,
            JsonValue val => val.GetValue<object?>(),

            null => null,
            var _ => throw new ArgumentOutOfRangeException(),
        };

    private static object UnwrapNumber(JsonElement element)
    {
        // Prefer the most specific integer type, fall back to double, then decimal
        if (element.TryGetInt32(out var i))
        {
            return i;
        }

        if (element.TryGetInt64(out var l))
        {
            return l;
        }

        if (element.TryGetUInt64(out var u))
        {
            return u;
        }

        if (element.TryGetDouble(out var d))
        {
            return d;
        }

        return element.GetDecimal();
    }
}
