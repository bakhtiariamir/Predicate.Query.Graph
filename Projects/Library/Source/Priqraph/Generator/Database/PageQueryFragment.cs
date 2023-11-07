using Priqraph.Query.Builders;
using System.Data;
using System.Data.SqlClient;

namespace Priqraph.Generator.Database;

public class PageQueryFragment : QueryFragment<Page>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private PageQueryFragment(Page? pagination = null)
    {
        Parameter = pagination ?? new Page(0, 10);
        SetText();
    }

    public void SetText() => _text = "OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

    public static PageQueryFragment Create(Page? pagination = null) => new(pagination);

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
