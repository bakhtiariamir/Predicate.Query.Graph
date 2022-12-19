using Parsis.Predicate.Sdk.DataType;
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

    protected override QueryPartType QueryPartType => QueryPartType.Paging;

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

    public void SetText() => _text = "OFFSET (@PageNumber-1) * @PageRows ROWS FETCH NEXT @PageRows ROWS ONLY";

    public static DatabasePagingClauseQueryPart Create(Page pagination) => new(pagination);

    public static DatabasePagingClauseQueryPart Create(int pageSize, int pageRows) => new(pageSize, pageRows);
}
