using Autofac;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;
using Priqraph.Info;
using Priqraph.Info.Database.Attribute;

namespace Priqraph.Setup;

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

    public static void RegisterSetting(ContainerBuilder builder) => builder.RegisterType<CacheInfoCollection>().As<ICacheInfoCollection>().SingleInstance();
}
