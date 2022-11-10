using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk;
public class InitConfig
{
    public static void InitGroupDataType()
    {
        WindowFunctionHelper.AddRelatedType(WindowFunctionType.AggregateCount, HavingType.Count);
        WindowFunctionHelper.AddRelatedType(WindowFunctionType.AggregateAverage,HavingType.Average);
        WindowFunctionHelper.AddRelatedType(WindowFunctionType.AggregateMax, HavingType.Max);
        WindowFunctionHelper.AddRelatedType(WindowFunctionType.AggregateMin, HavingType.Min);
        WindowFunctionHelper.AddRelatedType(WindowFunctionType.AggregateSum, HavingType.Sum);
    }
}
