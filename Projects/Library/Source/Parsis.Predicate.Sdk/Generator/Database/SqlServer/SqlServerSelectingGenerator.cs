using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler;
using Parsis.Predicate.Sdk.Helper;
using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Exception;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;

public class SqlServerSelectingGenerator<TObject> : Visitor<DatabaseColumnsClauseQueryPart<TObject>, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IColumnPropertyInfo> where TObject : class
{
    protected override IDatabaseCacheInfoCollection CacheObjectCollection
    {
        get;
    }
    protected override IDatabaseObjectInfo ObjectInfo
    {
        get;
    }

    protected override ParameterExpression ParameterExpression
    {
        get;
    }

    public SqlServerSelectingGenerator(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression parameterExpression)
    {
        ObjectInfo = objectInfo;
        ParameterExpression = parameterExpression;
        CacheObjectCollection = cacheObjectCollection;
    }

    public DatabaseColumnsClauseQueryPart<TObject> Generate(Expression expression) => Visit(expression);

    protected override DatabaseColumnsClauseQueryPart<TObject> VisitConvert(UnaryExpression expression)
    {
        return Visit(expression.Operand);
    }

    protected override DatabaseColumnsClauseQueryPart<TObject> VisitMember(MemberExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart<TObject>.Create(fields);
    }

    protected override DatabaseColumnsClauseQueryPart<TObject> VisitParameter(ParameterExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart<TObject>.Create(fields);
    }

    protected override DatabaseColumnsClauseQueryPart<TObject> VisitNewArray(NewArrayExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart<TObject>.Create(fields);
    }
}
