using Priqraph.DataType;

namespace Priqraph.Contract;

public interface IQueryIssue
{
    public DatabasePartIssueKey Key
    {
        get;
        set;
    }

    public QueryPartType QueryPartType
    {
        get;
        set;
    }

    public IPropertyInfo? ColumnPropertyInfo
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }
}

public enum DatabasePartIssueKey
{
    Unknown = 1
}
