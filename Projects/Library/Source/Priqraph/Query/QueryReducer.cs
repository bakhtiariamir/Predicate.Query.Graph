using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;

namespace Priqraph.Query
{
    internal  class QueryReducer<TObject> where TObject : IQueryableObject
    {
        private readonly ColumnPredicateReducer<TObject> _objectSelecting;
        private readonly FilterPredicateReducer<TObject> _objectFiltering;
        private IQuery<TObject> _query;

        private QueryReducer(IQuery<TObject> query)
        {
            _objectSelecting = new ColumnPredicateReducer<TObject>();
            _objectFiltering = new FilterPredicateReducer<TObject>();
            _query = query;
        }

        public static QueryReducer<TObject> Init(IQuery<TObject> query) => new(query);

        public QueryReducer<TObject> Reduce()
        {
            Enum.GetValues(typeof(ReduceType)).Cast<ReduceType>().ToList().ForEach(type =>
            {
                _query = _objectSelecting.Reduce(_query, type);
                _query = _objectFiltering.Reduce(_query, type);
            });
            return this;
        }

        public IQuery<TObject> Return() => _query;
    }
}
