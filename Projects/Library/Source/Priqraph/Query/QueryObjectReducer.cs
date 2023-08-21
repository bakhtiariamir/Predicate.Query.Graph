using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query
{
    public class QueryObjectReducer<TObject> where TObject : IQueryableObject
    {
        private QueryObjectSelectingReducer<TObject> _objectSelecting;
        private QueryObjectFilteringReducer<TObject> _objectFiltering;
        private QueryObject<TObject> _query;

        private QueryObjectReducer(QueryObject<TObject> query)
        {
            _objectSelecting = new QueryObjectSelectingReducer<TObject>();
            _objectFiltering = new QueryObjectFilteringReducer<TObject>();
            _query = query;
        }

        public static QueryObjectReducer<TObject> Init(QueryObject<TObject> query) => new(query);

        public QueryObjectReducer<TObject> Reduce()
        {
            Enum.GetValues(typeof(ReduceType)).Cast<ReduceType>().ToList().ForEach(type =>
            {
                _query = _objectSelecting.Reduce(_query, type);
                _query = _objectFiltering.Reduce(_query, type);
            });
            return this;
        }

        public QueryObject<TObject> Return() => _query;
    }
}
