namespace Parsis.Predicate.Sdk.Contract;
public interface ICacheObjectInfo<TObjectInfo> : ICacheObjectInfo
{
    TObjectInfo? GetObjectInfo();

    void SaveObjectInfo(TObjectInfo objectInfo);
}


public interface ICacheObjectInfo
{

}
