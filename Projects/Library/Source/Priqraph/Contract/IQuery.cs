using Priqraph.Builder.Database;

namespace Priqraph.Contract;

public interface IQuery<TObject, out TQueryResult> where TObject : IQueryableObject
{
    TQueryResult Build(IQueryObject<TObject> query);
}

public interface ISqlServerQuery<TObject> : IQuery<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{

}

public interface ISqlServerQueryBuilder<TObject> : IQueryBuilder<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{
}