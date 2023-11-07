using System;

namespace Priqraph.Manager.Result.Cache
{
    public class CacheQueryObject
    {
        public IEnumerable<CacheCommandQueryObject>? CommandQueryObject
        {
            get;
            set;
        }

        public CacheGetDataQueryObject? DataQueryObject
        {
            get;
            set;
        }
    }
}
