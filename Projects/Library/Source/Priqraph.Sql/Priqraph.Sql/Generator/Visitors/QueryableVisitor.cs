using System.Linq.Expressions;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Generator.Database;
using Priqraph.Info;

namespace Priqraph.Sql.Generator.Visitors
{
	public class QueryableVisitor<TObject> : DatabaseQueryableVisitor where TObject : IQueryableObject
	{
		public QueryableVisitor(ParameterExpression parameter, ICacheInfoCollection cacheInfoCollection, IDatabaseObjectInfo objectInfo) : base(parameter, cacheInfoCollection, objectInfo)
		{

		}

		protected override DatabaseQueryResult VisitMember(MemberExpression expression)
		{
			var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
			//return ColumnQueryFragment.Create(fields);
			return base.VisitMember(expression);
		}

		protected override DatabaseQueryResult VisitAndAlso(BinaryExpression expression)
		{
			return base.VisitAndAlso(expression);
		}

		protected override DatabaseQueryResult VisitOrElse(BinaryExpression expression)
		{
			return base.VisitOrElse(expression);
		}

		protected override DatabaseQueryResult VisitEqual(BinaryExpression expression)
		{
			return base.VisitEqual(expression);
		}

		protected override DatabaseQueryResult VisitNot(UnaryExpression expression)
		{
			return base.VisitNot(expression);
		}

		protected override DatabaseQueryResult VisitNotEqual(BinaryExpression expression)
		{
			return base.VisitNotEqual(expression);
		}

		protected override DatabaseQueryResult VisitGreaterThan(BinaryExpression expression)
		{
			var left = Visit(expression.Left);
			var right = Visit(expression.Right);
			return base.VisitGreaterThan(expression);
		}

		protected override DatabaseQueryResult VisitGreaterThanOrEqual(BinaryExpression expression)
		{
			return base.VisitGreaterThanOrEqual(expression);
		}

		protected override DatabaseQueryResult VisitLessThan(BinaryExpression expression)
		{
			return base.VisitLessThan(expression);
		}

		protected override DatabaseQueryResult VisitLessThanOrEqual(BinaryExpression expression)
		{
			return base.VisitLessThanOrEqual(expression);
		}


		protected override DatabaseQueryResult VisitWhere(MethodCallExpression expression)
		{
			var left = Visit(expression.Arguments[0]);
			var right = Visit((LambdaExpression)VisitQuote(expression.Arguments[1]));
			return default;
		}

		protected override DatabaseQueryResult VisitOrderBy(MethodCallExpression expression)
		{
			return base.VisitOrderBy(expression);
		}

		protected override DatabaseQueryResult VisitOrderByDescending(MethodCallExpression expression)
		{
			return base.VisitOrderByDescending(expression);
		}

		protected override DatabaseQueryResult VisitTake(MethodCallExpression expression)
		{
			return base.VisitTake(expression);
		}

		protected override DatabaseQueryResult VisitSkip(MethodCallExpression expression)
		{
			return base.VisitSkip(expression);
		}

		protected override DatabaseQueryResult VisitSelect(MethodCallExpression expression)
		{
			var left = Visit(expression.Arguments[0]);
			var right = Visit((LambdaExpression)VisitQuote(expression.Arguments[1]));
			return default;
		}

		protected override DatabaseQueryResult VisitConstant(ConstantExpression expression, string? memberName = null, MemberExpression? memberExpression = null)
		{
			if (expression.Value is IQueryable<TObject> query)
			{
				//GetFrom CacheObjectInfo
				var objects = query.ElementType.Name;
			}
			else
			{

			}

			return default;
		}
	}
}
