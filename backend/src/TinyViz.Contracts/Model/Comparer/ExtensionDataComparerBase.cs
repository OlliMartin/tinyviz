using TinyViz.Contracts.Model.ChartSpecification;

namespace TinyViz.Contracts.Model.Comparer;

public abstract class ExtensionDataComparerBase<TModel> : IEqualityComparer<IHasExtensionData<TModel>>
    where TModel : IHasExtensionData<TModel>
{
    public bool Equals(IHasExtensionData<TModel>? x, IHasExtensionData<TModel>? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        var xWithExtData = x.ToPrimitiveExtensionData();
        var yWithExtData = y.ToPrimitiveExtensionData();

        if (xWithExtData.ExtensionData.Count != yWithExtData.ExtensionData.Count)
        {
            return false;
        }

        return PropertiesEqual(xWithExtData, yWithExtData)
            && PrimitiveDictionaryComparer.Instance.Equals(xWithExtData.ExtensionData, yWithExtData.ExtensionData);
    }

    public int GetHashCode(IHasExtensionData<TModel> obj)
    {
        var objWithExtData = obj.ToPrimitiveExtensionData();

        HashCode result = default;
        result.Add(GetPropertiesHashCode(objWithExtData));
        result.Add(PrimitiveDictionaryComparer.Instance.GetHashCode(objWithExtData.ExtensionData));

        return result.ToHashCode();
    }

    protected abstract bool PropertiesEqual(TModel x, TModel y);

    protected abstract int GetPropertiesHashCode(TModel obj);
}
