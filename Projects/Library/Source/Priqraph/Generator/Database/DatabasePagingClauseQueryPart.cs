using Priqraph.Query;
using System.Data;
using System.Data.SqlClient;

namespace Priqraph.Generator.Database;

public class DatabasePagingClauseQueryPart : DatabaseQueryPart<Page>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    public DatabasePagingClauseQueryPart(int skip, int take)
    {
        Parameter = new Page(skip, take);
        SetText();
    }

    public DatabasePagingClauseQueryPart(Page pagination)
    {
        Parameter = pagination;
        SetText();
    }

    public void SetText() => _text = "OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

    public static DatabasePagingClauseQueryPart Create(Page pagination) => new(pagination);

    public static DatabasePagingClauseQueryPart Create(int skip, int take) => new(skip, take);

    public static IEnumerable<SqlParameter>? GetParameters(Page? page)
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
