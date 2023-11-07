using Priqraph.Contract;
using System.Linq.Expressions;

namespace Priqraph.Query.NewStructure.Handlers;

public class EntityHandler<TObject> : WriterHandler<TObject>, IEntityHandler<TObject> where TObject : IQueryableObject
{
    public override void Handle(IQueryObject<TObject> queryObject)
    {

    }
}


