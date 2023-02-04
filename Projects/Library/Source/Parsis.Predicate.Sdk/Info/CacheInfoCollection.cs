using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Info
{
    public abstract class CacheInfoCollection : ICacheInfoCollection
    {
        protected abstract string CacheKey
        {
            get;
        }

        public abstract void InitCache(string objectType, object value);

        public abstract bool TryRemove(string objectType, out object? value);

        public abstract bool RemoveCache(string objectType);

        public abstract bool TryGet(string objectType, out object? value);

        public abstract string GetKey(string objectType);
    }
}
