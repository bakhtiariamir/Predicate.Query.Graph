namespace Parsis.Predicate.Sdk.Contract;
public interface ICacheObjectInfo<TObjectInfo, TObject> where TObjectInfo : IObjectInfo<TObject> where TObject : class
{
    TObjectInfo? GetObjectInfo();

    void SaveObjectInfo(TObjectInfo objectInfo);
}

