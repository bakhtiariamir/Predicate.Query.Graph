using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query
{
    public class QueryObjectReducer<TObject, TQueryType> where TObject : IQueryableObject
        where TQueryType : Enum
    {
        private QueryObjectSelectingReducer<TObject, TQueryType> _objectSelecting;
        private QueryObjectFilteringReducer<TObject, TQueryType> _objectFiltering;
        private QueryObject<TObject, TQueryType> _query;

        private QueryObjectReducer(QueryObject<TObject, TQueryType> query)
        {
            _objectSelecting = new QueryObjectSelectingReducer<TObject, TQueryType>();
            _objectFiltering = new QueryObjectFilteringReducer<TObject, TQueryType>();
            _query = query;
        }

        public static QueryObjectReducer<TObject, TQueryType> Init(QueryObject<TObject, TQueryType> query) => new(query);

        public QueryObjectReducer<TObject, TQueryType> Reduce()
        {
            Enum.GetValues(typeof(ReduceType)).Cast<ReduceType>().ToList().ForEach(type =>
            {
                _query = _objectSelecting.Reduce(_query, type);
                _query = _objectFiltering.Reduce(_query, type);
            });
            return this;
        }

        public QueryObject<TObject, TQueryType> Return() => _query;
    }
}
