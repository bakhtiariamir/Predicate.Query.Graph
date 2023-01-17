﻿using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Generator.Database;
using System.Data.SqlClient;
using System.Text;

namespace Parsis.Predicate.Sdk.Helper;

public static class DatabaseQueryHelper
{
    public static string GetSelectQuery(this DatabaseQueryPartCollection queryParts, out ICollection<SqlParameter>? parameters)
    {
        if (queryParts.Columns == null) throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        parameters = queryParts.SelectParameters().ToArray();

        var select = new StringBuilder();
        select.Append($"SELECT {queryParts.Columns.Text} ");
        select.Append($"FROM {queryParts.DatabaseObjectInfo} ");
        select.Append($"{queryParts.JoinClause?.Text} ");
        select.Append(queryParts.WhereClause != null ? $"WHERE {queryParts.WhereClause.Text} " : "");
        select.Append(queryParts.GroupByClause != null ? $"GROUP BY {queryParts.GroupByClause.Text} " : "");
        select.Append(queryParts.GroupByClause is {Having: { }} ? $"HAVING {queryParts.GroupByClause?.Having} " : "");
        select.Append(queryParts.OrderByClause != null ? $"{queryParts.OrderByClause.Text}" : "");

        return select.ToString();
    }

    public static string GetCommandQuery(this DatabaseQueryPartCollection queryParts, out ICollection<SqlParameter> parameters)
    {
        if (queryParts.Command == null) throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        parameters = queryParts.Command.SqlParameters;

        if (parameters != null && !parameters.Any())
            throw new NotSupported(ExceptionCode.DatabaseQueryGenerator);

        return queryParts.Command.OperationType switch {
            DatabaseQueryOperationType.Insert => queryParts.Command.GetInsertQuery(),
            DatabaseQueryOperationType.Delete => queryParts.Command.GetDeleteQuery(),
            DatabaseQueryOperationType.Update => queryParts.Command.GetUpdateQuery(),
            DatabaseQueryOperationType.Merge => queryParts.Command.GetMergeQuery(),
            DatabaseQueryOperationType.Select or _ => throw new NotSupported(ExceptionCode.QueryGenerator)
        };
    }

    private static string GetInsertQuery(this DatabaseCommandQueryPart command)
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
                    throw new NotSupported(ExceptionCode.DatabaseObjectInfo); //todo

                insert.Append($"VALUES {value}");
                break;
            case CommandValueType.Bulk:

                break;
        }

        return insert.ToString();
    }

    private static string GetUpdateQuery(this DatabaseCommandQueryPart command)
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
                            throw new NotSupported("asd"); // todo

                        if (commandParts.TryGetValue("RecordsWhere", out var whereList))
                        {
                            if (whereList is ICollection<Tuple<int, string>> recordsWhere && recordsValue!.Count > 0)
                            {
                                foreach (var (index, columns) in recordsValue)
                                {
                                    update.Append($"UPDATE {selector} ");
                                    update.Append($"SET {columns} ");
                                    var where = recordsWhere.FirstOrDefault(item => item.Item1 == index)?.Item2 ?? throw new NotSupported("asd"); // todo

                                    update.Append($"WHERE {where}; ");
                                }
                            }
                        }
                        else
                            throw new NotSupported(""); // todo
                    }
                    else if (commandParts.TryGetValue("Values", out var values))
                    {
                        update.Append($"UPDATE {selector} ");
                        update.Append($"SET {values} ");

                        if (commandParts.TryGetValue("Where", out var where))
                            update.Append($"WHERE {where}");
                    }
                    else
                        throw new NotSupported(""); // todo

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

    private static string GetDeleteQuery(this DatabaseCommandQueryPart command)
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

    private static string GetMergeQuery(this DatabaseCommandQueryPart command)
    {
        return string.Empty;
    }
}
