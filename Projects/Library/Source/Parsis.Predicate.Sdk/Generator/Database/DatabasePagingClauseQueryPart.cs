using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Generator.Database;

public class DatabasePagingClauseQueryPart : DatabaseQueryPart<Page>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    public DatabasePagingClauseQueryPart(int pageNumber, int pageRows)
    {
        Parameter = new Page(pageNumber, pageRows);
        SetText();
    }

    public DatabasePagingClauseQueryPart(Page pagination)
    {
        Parameter = pagination;
        SetText();
    }

    public void SetText() => _text = "OFFSET (@Skip-1) * @Take ROWS FETCH NEXT @Take ROWS ONLY";

    public static DatabasePagingClauseQueryPart Create(Page pagination) => new(pagination);

    public static DatabasePagingClauseQueryPart Create(int pageSize, int pageRows) => new(pageSize, pageRows);
}
