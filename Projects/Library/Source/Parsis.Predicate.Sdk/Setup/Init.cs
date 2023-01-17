using Autofac;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Info;

namespace Parsis.Predicate.Sdk.Setup;

public class Setup
{
    private Setup()
    {
    }

    public static Setup Init() => new();

    public Setup DefineDataType()
    {
        WindowFunctionType.AggregateCount.AddRelatedType(HavingType.Count);
        WindowFunctionType.AggregateAverage.AddRelatedType(HavingType.Average);
        WindowFunctionType.AggregateMax.AddRelatedType(HavingType.Max);
        WindowFunctionType.AggregateMin.AddRelatedType(HavingType.Min);
        WindowFunctionType.AggregateSum.AddRelatedType(HavingType.Sum);

        return this;
    }

    public static void SetupIoc(ContainerBuilder builder) => builder.RegisterType<DatabaseCacheInfoCollection>().As<IDatabaseCacheInfoCollection>().SingleInstance();
}
