using Autofac;
using Priqraph.Contract;
using Priqraph.Sql.Manager;

namespace Priqraph.Sql;
public static class Setup
{
    public static void RegisterSetting(ContainerBuilder builder) => builder.RegisterGeneric(typeof(SqlServerQueryBuilder<>)).As(typeof(ISqlServerQueryBuilder<>)).SingleInstance();
}