using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyViz.Contracts.Model.ChartSpecification;

[UsedImplicitly]
public record TraceDescriptor
{
    [JsonRequired]
    public required string TypeName { get; init; }

    public static IEqualityComparer<TraceDescriptor> JsonComparer { get; } = new JsonEqualityComparer();

    [JsonExtensionData]
    public Dictionary<string, object?> AdditionalData { get; set; } = new();

    private class JsonEqualityComparer : IEqualityComparer<TraceDescriptor>
    {
        public bool Equals(TraceDescriptor? x, TraceDescriptor? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            if (x.TypeName != y.TypeName)
            {
                return false;
            }

            if (x.AdditionalData.Count != y.AdditionalData.Count)
            {
                return false;
            }

            foreach (var (key, xValue) in x.AdditionalData)
            {
                if (!y.AdditionalData.TryGetValue(key, out var yValue))
                {
                    return false;
                }

                if (!ValuesEqual(xValue, yValue))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(TraceDescriptor obj)
        {
            var hash = new HashCode();
            hash.Add(obj.TypeName);

            foreach (var key in obj.AdditionalData.Keys.Order())
            {
                hash.Add(key);
                hash.Add(ComputeValueHashCode(obj.AdditionalData[key]));
            }

            return hash.ToHashCode();
        }

        private static bool ValuesEqual(object? x, object? y)
        {
            // Unwrap JsonElements on either side
            var xElement = ToJsonElement(x);
            var yElement = ToJsonElement(y);

            return (xElement, yElement) switch
            {
                (JsonElement xe, JsonElement ye) => JsonElementsEqual(xe, ye),
                (JsonElement xe, null) => JsonElementEqualsPrimitive(xe, y),
                (null, JsonElement ye) => JsonElementEqualsPrimitive(ye, x),
                var _ => Equals(x, y),
            };
        }

        private static JsonElement? ToJsonElement(object? value) => value is JsonElement je ? je : null;

        private static bool JsonElementsEqual(JsonElement x, JsonElement y)
        {
            if (x.ValueKind != y.ValueKind)
            {
                return false;
            }

            return x.ValueKind switch
            {
                JsonValueKind.Object => ObjectsEqual(x, y),
                JsonValueKind.Array => ArraysEqual(x, y),
                JsonValueKind.String => x.GetString() == y.GetString(),
                JsonValueKind.Number => x.GetDecimal() == y.GetDecimal(),
                JsonValueKind.True or JsonValueKind.False => x.GetBoolean() == y.GetBoolean(),
                JsonValueKind.Null or JsonValueKind.Undefined => true,
                var _ => false,
            };
        }

        private static bool ObjectsEqual(JsonElement x, JsonElement y)
        {
            var xProps = x.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);
            var yProps = y.EnumerateObject().ToDictionary(p => p.Name, p => p.Value);

            if (xProps.Count != yProps.Count)
            {
                return false;
            }

            foreach (var (key, xVal) in xProps)
            {
                if (!yProps.TryGetValue(key, out var yVal))
                {
                    return false;
                }

                if (!JsonElementsEqual(xVal, yVal))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ArraysEqual(JsonElement x, JsonElement y)
        {
            var xItems = x.EnumerateArray().ToList();
            var yItems = y.EnumerateArray().ToList();

            if (xItems.Count != yItems.Count)
            {
                return false;
            }

            return !xItems.Where((t, i) => !JsonElementsEqual(t, yItems[i])).Any();
        }

        // Compare a JsonElement against a raw CLR primitive (e.g. string, int, bool)
        private static bool JsonElementEqualsPrimitive(JsonElement element, object? primitive) =>
            element.ValueKind switch
            {
                JsonValueKind.String => primitive is string s && element.GetString() == s,
                JsonValueKind.Number => primitive switch
                {
                    byte v => element.TryGetByte(out var b) && b == v,
                    short v => element.TryGetInt16(out var s) && s == v,
                    int v => element.TryGetInt32(out var i) && i == v,
                    long v => element.TryGetInt64(out var l) && l == v,
                    float v => element.TryGetSingle(out var f) && f == v,
                    double v => element.TryGetDouble(out var d) && d == v,
                    decimal v => element.TryGetDecimal(out var dec) && dec == v,
                    var _ => false,
                },
                JsonValueKind.True => primitive is true,
                JsonValueKind.False => primitive is false,
                JsonValueKind.Null => primitive is null,
                var _ => false,
            };

        private static int ComputeValueHashCode(object? value) =>
            value switch
            {
                JsonElement je => ComputeJsonElementHashCode(je),
                null => 0,
                var _ => value.GetHashCode(),
            };

        private static int ComputeJsonElementHashCode(JsonElement element) =>
            element.ValueKind switch
            {
                JsonValueKind.String => element.GetString()?.GetHashCode() ?? 0,
                JsonValueKind.Number => element.GetDecimal().GetHashCode(),
                JsonValueKind.True => true.GetHashCode(),
                JsonValueKind.False => false.GetHashCode(),
                JsonValueKind.Null or JsonValueKind.Undefined => 0,
                JsonValueKind.Object => element
                    .EnumerateObject()
                    .OrderBy(p => p.Name)
                    .Aggregate(
                        new HashCode(),
                        (h, p) =>
                        {
                            h.Add(p.Name);
                            h.Add(ComputeJsonElementHashCode(p.Value));
                            return h;
                        },
                        h => h.ToHashCode()
                    ),
                JsonValueKind.Array => element
                    .EnumerateArray()
                    .Aggregate(
                        new HashCode(),
                        (h, e) =>
                        {
                            h.Add(ComputeJsonElementHashCode(e));
                            return h;
                        },
                        h => h.ToHashCode()
                    ),
                var _ => 0,
            };
    }
}
