using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Builders;
using System;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class CommandPredicate<TObject> where TObject : IQueryableObject
    {
        public CommandValueType CommandValueType
        {
            get;
        }

        public QueryPartType CommandPartType
        {
            get;
        }
        public ReturnType ReturnType
        {
            get;
            set;
        }

        public ICollection<Expression<Func<TObject, TObject>>>? ObjectPredicate
        {
            get;
            private set;
        }

        public ICollection<Expression<Func<TObject, IEnumerable<TObject>>>>? ObjectsPredicate
        {
            get;
            private set;
        }

        public FilterPredicateBuilder<TObject>? Filter
        {
            get;
            private set;
        }

        public CommandPredicate(QueryPartType commandPartType, CommandValueType commandValueType, ReturnType returnType = ReturnType.None)
        {
            CommandValueType = commandValueType;
            CommandPartType = commandPartType;
            ReturnType = returnType;
        }

        public void SetObjectPredicate(ICollection<Expression<Func<TObject, TObject>>> predicate) => ObjectPredicate = predicate;

        public void AddObjectPredicate(Expression<Func<TObject, TObject>> predicate)
        {
            if (ObjectPredicate == null) SetObjectPredicate(new[] { predicate });
            else ObjectPredicate.Add(predicate);
        }

        public void AddObjectsPredicate(Expression<Func<TObject, IEnumerable<TObject>>> predicates)
        {
            if (ObjectsPredicate == null) SetObjectsPredicate(new[] { predicates });
            else ObjectsPredicate.Add(predicates);
        }

        public void SetObjectsPredicate(ICollection<Expression<Func<TObject, IEnumerable<TObject>>>> predicates) => ObjectsPredicate = predicates;

        public void SetObjectFiltering(FilterPredicateBuilder<TObject> filter) => Filter = filter;
    }
}
