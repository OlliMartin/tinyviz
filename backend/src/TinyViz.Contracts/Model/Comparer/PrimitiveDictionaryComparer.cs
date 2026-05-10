using System.Collections;

namespace TinyViz.Contracts.Model.Comparer;

public class PrimitiveDictionaryComparer : IEqualityComparer<Dictionary<string, object?>>
{
    public static readonly IEqualityComparer<Dictionary<string, object?>> Instance = new PrimitiveDictionaryComparer();

    private PrimitiveDictionaryComparer() { }

    public bool Equals(Dictionary<string, object?>? x, Dictionary<string, object?>? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var (key, xValue) in x)
        {
            if (!y.TryGetValue(key, out var yValue))
            {
                return false;
            }

            if (!PrimitiveValuesEqual(xValue, yValue))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(Dictionary<string, object?> obj)
    {
        var hash = new HashCode();

        foreach (var key in obj.Keys.Order())
        {
            hash.Add(key);
            hash.Add(ComputePrimitiveHashCode(obj[key]));
        }

        return hash.ToHashCode();
    }

    private static bool PrimitiveValuesEqual(object? x, object? y) =>
        (x, y) switch
        {
            (null, null) => true,
            (null, var _) or (var _, null) => false,
            (IDictionary<string, object?> xd, IDictionary<string, object?> yd) => DictionariesEqual(xd, yd),
            (IList xl, IList yl) => ListsEqual(xl, yl),
            (IConvertible xc, IConvertible yc) when xc.GetTypeCode() != yc.GetTypeCode() => NumericEqual(xc, yc),
            var _ => Equals(x, y),
        };

    private static bool DictionariesEqual(IDictionary<string, object?> x, IDictionary<string, object?> y)
    {
        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var (key, xValue) in x)
        {
            if (!y.TryGetValue(key, out var yValue))
            {
                return false;
            }

            if (!PrimitiveValuesEqual(xValue, yValue))
            {
                return false;
            }
        }

        return true;
    }

    private static bool ListsEqual(IList x, IList y)
    {
        if (x.Count != y.Count)
        {
            return false;
        }

        for (var i = 0; i < x.Count; i++)
        {
            if (!PrimitiveValuesEqual(x[i], y[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool NumericEqual(IConvertible x, IConvertible y)
    {
        try
        {
            return IsFloating(x.GetTypeCode()) || IsFloating(y.GetTypeCode())
                ? Convert.ToDecimal(x) == Convert.ToDecimal(y)
                : Convert.ToInt64(x) == Convert.ToInt64(y);
        }
        catch (OverflowException)
        {
            return false;
        }
    }

    private static int ComputePrimitiveHashCode(object? value) =>
        value switch
        {
            null => 0,
            IDictionary<string, object?> d => d
                .Keys.Order()
                .Aggregate(
                    new HashCode(),
                    (h, k) =>
                    {
                        h.Add(k);
                        h.Add(ComputePrimitiveHashCode(d[k]));
                        return h;
                    },
                    h => h.ToHashCode()
                ),
            IList l => Enumerable
                .Range(start: 0, l.Count)
                .Aggregate(
                    new HashCode(),
                    (h, i) =>
                    {
                        h.Add(ComputePrimitiveHashCode(l[i]));
                        return h;
                    },
                    h => h.ToHashCode()
                ),
            IConvertible c when IsFloating(c.GetTypeCode()) => Convert.ToDecimal(c).GetHashCode(),
            IConvertible c => Convert.ToInt64(c).GetHashCode(),
            var _ => value.GetHashCode(),
        };

    private static bool IsFloating(TypeCode code) => code is TypeCode.Single or TypeCode.Double or TypeCode.Decimal;
}
