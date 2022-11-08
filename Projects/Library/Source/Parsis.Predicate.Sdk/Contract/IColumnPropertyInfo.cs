namespace Parsis.Predicate.Sdk.Contract;
public interface IColumnPropertyInfo : IPropertyInfo
{
    bool IsNullable
    {
        get;
    }

    string ColumnName
    {
        get;
    }

    string Alias
    {
        get;
    }
}
