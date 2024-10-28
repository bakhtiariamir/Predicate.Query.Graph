﻿using Priqraph.Generator.Database;
using Priqraph.Query.Builders;
using System.Data;
using System.Data.SqlClient;

namespace Priqraph.Neo4j.Generator;
public class Neo4jPageQueryFragment : DatabasePageQueryFragment
{

    private Neo4jPageQueryFragment(PageClause? pagination = null)
    {
        Parameter = pagination ?? new PageClause(0, 10);
        SetText();
    }

    public void SetText() => Text = "OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

    public static Neo4jPageQueryFragment Create(PageClause? pagination = null) => new(pagination);

    public static IEnumerable<SqlParameter>? GetParameters(PageClause? page)
    {
        if (page is null)
            yield break;

        yield return new SqlParameter
        {
            ParameterName = "Take",
            SqlDbType = SqlDbType.Int,
            Value = page.Take
        };
        yield return new SqlParameter
        {
            ParameterName = "Skip",
            SqlDbType = SqlDbType.Int,
            Value = page.Skip
        };
    }
}
