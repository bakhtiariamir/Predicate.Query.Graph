using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Info
{
    public abstract class CacheInfoCollection<TObjectInfo> : ICacheInfoCollection<TObjectInfo>
    {
        protected abstract string CacheKey
        {
            get;
        }

        public abstract TObjectInfo InitCache(string objectType, TObjectInfo value);
        public abstract bool TryRemove(string objectType, out TObjectInfo? value);
        public abstract bool RemoveCache(string objectType);
        public abstract bool TryGet(string objectType, out IDatabaseObjectInfo? value);

        public abstract string GetKey(string objectType);
    }
}
