namespace Parsis.Predicate.Sdk.Contract;
public interface IDatabaseObjectInfo<TObject> : IObjectInfo<TObject> where TObject : class
{
    string Table
    {
        get;
    }

    string Schema
    {
        get;
    }


}
