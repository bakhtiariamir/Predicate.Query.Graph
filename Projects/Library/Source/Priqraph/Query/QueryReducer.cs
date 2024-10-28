using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;

namespace Priqraph.Query
{
    public  class QueryReducer<TObject, TQueryObject, TEnum> 
        where TObject : IQueryableObject 
        where TQueryObject : IQuery<TObject, TEnum>
        where TEnum : struct, IConvertible    
    {
        private readonly ColumnPredicateReducer<TObject, TQueryObject, TEnum> _objectSelecting;
        private readonly FilterPredicateReducer<TObject, TQueryObject, TEnum> _objectFiltering;
        private TQueryObject _query;

        private QueryReducer(TQueryObject query)
        {
            _objectSelecting = new ColumnPredicateReducer<TObject, TQueryObject, TEnum>();
            _objectFiltering = new FilterPredicateReducer<TObject, TQueryObject, TEnum>();
            _query = query;
        }

        public static QueryReducer<TObject, TQueryObject, TEnum> Init(TQueryObject query) => new(query);

        public QueryReducer<TObject, TQueryObject, TEnum> Reduce()
        {
            Enum.GetValues(typeof(ReduceType)).Cast<ReduceType>().ToList().ForEach(type =>
            {
                _query = _objectSelecting.Reduce(_query, type);
                _query = _objectFiltering.Reduce(_query, type);
            });
            return this;
        }

        public TQueryObject Return() => _query;
    }
}
