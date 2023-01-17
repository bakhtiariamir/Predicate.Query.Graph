namespace Parsis.Predicate.Sdk.Contract;

public interface IQueryableObject<out TPropertyInfo> : IQueryableObject where TPropertyInfo : IPropertyInfo
{
    IObjectInfo<TPropertyInfo> GetObjectInfo();
}

public interface IQueryableObject
{
}
