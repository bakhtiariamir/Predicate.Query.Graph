using Priqraph.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Priqraph.Contract
{
    public interface IQueryOperationFactory<TObject, TResult> where TObject : IQueryableObject
    {
        IQuery<TObject, TResult> QueryProvider(ICacheInfoCollection cacheInfoCollection, QueryProvider provider);
    }
}
