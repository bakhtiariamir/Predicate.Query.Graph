using Autofac;
using Priqraph.Neo4j.Builder;

namespace Priqraph.Neo4j;

public class Setup
{
    public static void RegisterSetting(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(Neo4jQueryBuilder<>))
            .As(typeof(INeo4jQueryBuilder<>)).SingleInstance();
    }
}