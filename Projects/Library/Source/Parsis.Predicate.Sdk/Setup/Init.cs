using System;
using Autofac;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Info.Database;

namespace Parsis.Predicate.Sdk.Setup;
public class Setting
{
    private Setting()
    {

    }
    public static Setting Init() => new();
    public Setting DefineDataType()
    {
        WindowFunctionType.AggregateCount.AddRelatedType(HavingType.Count);
        WindowFunctionType.AggregateAverage.AddRelatedType(HavingType.Average);
        WindowFunctionType.AggregateMax.AddRelatedType(HavingType.Max);
        WindowFunctionType.AggregateMin.AddRelatedType(HavingType.Min);
        WindowFunctionType.AggregateSum.AddRelatedType(HavingType.Sum);

        return this;
    }

    public Setting SetupIoc(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(IDatabaseCacheObjectInfo<>)).As(typeof(DatabaseCacheObjectInfo<>)).SingleInstance();

        return this;
    }

}


public enum LoadingType
{
    Lazy = 1,
    Eager = 2
}