namespace Parsis.Predicate.Sdk.DataType;

public enum DatabaseFieldType
{
    Column = 1,
    AggregateWindowFunction = 2,
    RankingWindowFunction = 3,
    ValueWindowFunction = 4,
    Functional = 5,
    NotMapped = 6
}
