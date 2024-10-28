using Priqraph.Builder.Database;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using System.Data.SqlClient;
using System.Text;

namespace Priqraph.Sql.Extensions;

internal static class QueryHelper
{
    public static string Select(this DatabaseQueryResult queryParts, out ICollection<SqlParameter>? parameters)
    {
        if (queryParts.ColumnFragment == null) throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        parameters = queryParts.SelectParameters().ToArray();

        var select = new StringBuilder();
        select.Append($"SELECT {queryParts.ColumnFragment.Text} ");
        select.Append($"FROM {queryParts.DatabaseObjectInfo} ");
        select.Append($"{queryParts.JoinFragment?.Text} ");
        select.Append(queryParts.FilterFragment != null ? $"WHERE {queryParts.FilterFragment.Text} " : "");
        select.Append(queryParts.GroupByFragment != null ? $"GROUP BY {queryParts.GroupByFragment.Text} " : "");
        select.Append(queryParts.GroupByFragment is { Having: { } } ? $"HAVING {queryParts.GroupByFragment?.Having} " : "");
        select.Append(queryParts.SortFragment != null ? $"{queryParts.SortFragment.Text} " : "");
        select.Append(queryParts.PageFragment != null ? queryParts.PageFragment.Text : "");
        return select.ToString();
    }

    public static string Command(this DatabaseQueryResult queryParts, out ICollection<SqlParameter> parameters)
    {
        if (queryParts.CommandFragment == null) throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        parameters = queryParts.CommandFragment.SqlParameters;

        if (parameters != null && !parameters.Any())
            throw new NotSupportedOperationException(ExceptionCode.DatabaseQueryGenerator);

        return queryParts.CommandFragment.OperationType switch
        {
            DatabaseQueryOperationType.Add => queryParts.CommandFragment.Insert(queryParts),
            DatabaseQueryOperationType.Remove => queryParts.CommandFragment.Delete(),
            DatabaseQueryOperationType.Edit => queryParts.CommandFragment.Update(queryParts),
            DatabaseQueryOperationType.Merge => queryParts.CommandFragment.Merge(),
            DatabaseQueryOperationType.GetData or _ => throw new NotSupportedOperationException(ExceptionCode.QueryGenerator)
        };
    }

    private static string Insert(this DatabaseCommandQueryFragment command, DatabaseQueryResult queryParts)
    {
        var commandParts = command.CommandParts;
        var insert = new StringBuilder();

        switch (command.CommandValueType)
        {
            case CommandValueType.Record:
                insert.Append($"INSERT INTO {commandParts["Selector"]} ");
                insert.Append($"({commandParts["Columns"]}) ");
                string? value;
                if (commandParts["Values"] is IEnumerable<string> values)
                    value = string.Join(", ", values);
                else
                    value = commandParts["Values"].ToString();

                if (value == null)
                    throw new NotSupportedOperationException(ExceptionCode.DatabaseObjectInfo); //todo

                insert.Append($"VALUES {value};");
                if (command.CommandParts.ContainsKey("result") && queryParts.ResultQuery != null)
                {
                    var resultQueryParts = queryParts.ResultQuery;
                    var select = Select(resultQueryParts, out var selectParameter);
                    //var select = new StringBuilder();
                    //select.Append($"SELECT {resultQueryParts.Columns.Text} ");
                    //select.Append($"FROM {resultQueryParts.DatabaseObjectInfo} ");
                    //select.Append(resultQueryParts.WhereClause != null ? $"WHERE {resultQueryParts.WhereClause.Text} ;" : ";");
                    insert.AppendLine();
                    insert.Append($"{commandParts["result"]};");
                    insert.AppendLine();
                    insert.AppendLine(select);
                }
                break;
            case CommandValueType.Bulk:

                break;
        }

        return insert.ToString();
    }

    private static string Update(this DatabaseCommandQueryFragment command, DatabaseQueryResult queryParts)
    {
        var commandParts = command.CommandParts;
        var update = new StringBuilder();

        switch (command.CommandValueType)
        {
            case CommandValueType.Record:
                {
                    var selector = commandParts["Selector"];
                    if (commandParts.TryGetValue("RecordsValue", out var valueList))
                    {
                        var recordsValue = valueList as ICollection<Tuple<int, string>>;
                        if (recordsValue == null && recordsValue!.Count == 0)
                            throw new NotSupportedOperationException("asd"); // todo

                        if (commandParts.TryGetValue("RecordsWhere", out var whereList))
                        {
                            if (whereList is ICollection<Tuple<int, string>> recordsWhere && recordsValue!.Count > 0)
                            {
                                foreach (var (index, columns) in recordsValue)
                                {
                                    update.Append($"UPDATE {selector} ");
                                    update.Append($"SET {columns} ");
                                    var where = recordsWhere.FirstOrDefault(item => item.Item1 == index)?.Item2 ?? throw new NotSupportedOperationException("asd"); // todo

                                    update.Append($"WHERE {where}; ");
                                }
                            }
                        }
                        else
                            throw new NotSupportedOperationException(""); // todo
                    }
                    else if (commandParts.TryGetValue("Values", out var values))
                    {
                        update.Append($"UPDATE {selector} ");
                        update.Append($"SET {values} ");

                        if (commandParts.TryGetValue("Where", out var where))
                            update.Append($"WHERE {where};");

                        if (command.CommandParts.ContainsKey("result") && queryParts.ResultQuery != null)
                        {
                            var resultQueryParts = queryParts.ResultQuery;
                            var select = Select(resultQueryParts, out var selectParameter);
                            //var select = new StringBuilder();
                            //select.Append($"SELECT {resultQueryParts.Columns.Text} ");
                            //select.Append($"FROM {resultQueryParts.DatabaseObjectInfo} ");
                            //select.Append(resultQueryParts.WhereClause != null ? $"WHERE {resultQueryParts.WhereClause.Text} " : "");
                            update.AppendLine();
                            update.Append(commandParts["result"]);
                            update.AppendLine();
                            update.AppendLine(select);
                        }
                    }
                    else
                        throw new NotSupportedOperationException(""); // todo

                    break;
                }
            case CommandValueType.Bulk:
                update.Append($"UPDATE {commandParts["Selectors"]} ");
                update.Append($"SET {commandParts["Values"]} ");
                update.Append($"{commandParts["Join"]} ");
                update.Append($"WHERE {commandParts["Where"]}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return update.ToString();
    }

    private static string Delete(this DatabaseCommandQueryFragment command)
    {
        var commandParts = command.CommandParts;
        var delete = new StringBuilder();

        var selector = commandParts["Selector"];

        switch (command.CommandValueType)
        {
            case CommandValueType.Record:
                delete.Append($"DELETE FROM {selector} ");
                delete.Append($"WHERE {commandParts["Where"]}");
                break;
            case CommandValueType.Bulk:
                delete.Append($"DELETE {selector} ");
                delete.Append($"{commandParts["Joins"]} ");
                delete.Append($"WHERE {commandParts["WHERE"]} ");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return delete.ToString();
    }

    private static string Merge(this DatabaseCommandQueryFragment command)
    {
        return string.Empty;
    }
}
