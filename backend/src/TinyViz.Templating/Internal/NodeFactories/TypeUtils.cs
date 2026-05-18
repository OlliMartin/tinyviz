namespace TinyViz.Templating.Internal.NodeFactories;

internal static class TypeUtils
{
    public static bool IsPrimitive(object? item) => item is string || item is int || item is float || item is double || item is bool;
}
