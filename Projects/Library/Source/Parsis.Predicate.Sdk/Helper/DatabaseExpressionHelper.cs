using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Info.Database;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseExpressionHelper
{
    public static IColumnPropertyInfo? GetProperty<TObject>(this Expression expression, IDatabaseObjectInfo<TObject> objectInfo)
    where TObject : class
    {
        if (expression is MemberExpression member)
        {
            if (member.Expression == null)
                //ToDo : Exception
                throw new System.Exception("asdasd");

            if (member.Expression.NodeType == ExpressionType.MemberAccess)
            {
                if (member.Expression is MemberExpression nestedMember)
                {
                    if (nestedMember.Expression == null)
                        throw new System.Exception("asdasd");

                    return nestedMember.Expression.NodeType switch
                    {
                        ExpressionType.MemberAccess => nestedMember.GetProperty<TObject>(objectInfo),
                        ExpressionType.Parameter => objectInfo.PropertyInfos.FirstOrDefault(item => item.Name == nestedMember.Member.Name),
                        _ => throw new System.Exception("asdasd")
                    };
                }
            }
            else
            {
                return objectInfo.PropertyInfos.FirstOrDefault(item => item.Name == member.Member.Name);
            }
        }
        
        throw new NotSupportedException($"ExpressionType: {expression.GetType().FullName}, is not supported.");
    }
    //ToDo : 
    /*
     * Portfolio.Predicate(x =>  x.Account.User.Username == "Akbar")
     * Select * from Portfolio
     * Inner Join Account on Account.AccountId = Portfolio.AccountId
     * Inner Join User on User.Id on Account.UserId
     * Where User.username = "Akbar"
     *
     */

    /*
     * Read predicate from QueryObject or IQueryable
     * Create Extension Method For IEnumerable and IQueryable for run script
     */
}
