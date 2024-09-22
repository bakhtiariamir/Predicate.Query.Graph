using Priqraph.Builder.Database;

namespace Priqraph.Contract;

public interface IQueryObject<TObject, out TQueryResult> where TObject : IQueryableObject
{
    TQueryResult Build(IQuery<TObject> query);
}

public interface ISqlServerQueryObject<TObject> : IQueryObject<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{

}

public interface ISqlServerQueryableQueryObject<TObject> : IQueryObject<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{

}

public interface ISqlServerQueryBuilder<TObject> : IQueryBuilder<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{
}