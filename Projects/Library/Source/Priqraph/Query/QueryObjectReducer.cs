using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;

namespace Priqraph.Query
{
    public class QueryObjectReducer<TObject> where TObject : IQueryableObject
    {
        private ColumnPredicateReducer<TObject> _objectSelecting;
        private FilterPredicateReducer<TObject> _objectFiltering;
        private IQueryObject<TObject> _query;

        private QueryObjectReducer(IQueryObject<TObject> query)
        {
            _objectSelecting = new ColumnPredicateReducer<TObject>();
            _objectFiltering = new FilterPredicateReducer<TObject>();
            _query = query;
        }

        public static QueryObjectReducer<TObject> Init(IQueryObject<TObject> query) => new(query);

        public QueryObjectReducer<TObject> Reduce()
        {
            Enum.GetValues(typeof(ReduceType)).Cast<ReduceType>().ToList().ForEach(type =>
            {
                _query = _objectSelecting.Reduce(_query, type);
                _query = _objectFiltering.Reduce(_query, type);
            });
            return this;
        }

        public IQueryObject<TObject> Return() => _query;
    }
}
