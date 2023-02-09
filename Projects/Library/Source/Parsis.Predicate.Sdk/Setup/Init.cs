﻿using Autofac;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Info;
using Parsis.Predicate.Sdk.Info.Database.Attribute;

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

    public static void RegisterSetting(ContainerBuilder builder) => builder.RegisterType<CacheInfoCollection>().As<ICacheInfoCollection>().SingleInstance();

    public static void CacheObjectInfo(ICacheInfoCollection cacheInfoCollection)
    {
        //DataSetInfoAttribute
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(IQueryableObject).IsAssignableFrom(p));
        foreach (var type in types.Where(item => item.Name == "IDomain"))
        {
            var objectAttribute = type.GetCustomAttributes(true);

            if (objectAttribute.Length == 0)
                cacheInfoCollection.GetLastObjectInfo(type);
            else
            {
                foreach (var attribute in objectAttribute)
                {
                    if (attribute is DataSetInfoAttribute infoAttribute)
                        cacheInfoCollection.GetLastDatabaseObjectInfo(type);
                }
            }
        }
    }
}
