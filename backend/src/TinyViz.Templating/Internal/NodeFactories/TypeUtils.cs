namespace TinyViz.Templating.Internal.NodeFactories;

internal static class TypeUtils
{
    public static bool IsPrimitive(object? item) =>
        item is null
        || item is string
        || item is short
        || item is byte
        || item is int
        || item is long
        || item is float
        || item is double
        || item is bool;
}
