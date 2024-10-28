namespace Priqraph.DataType;

public enum DatabaseQueryOperationType
{
    GetData = 1,
    GetCount = 2,
    Add = 3,
    Edit = 4,
    Remove = 5,
    Merge = 6,
    Script = 7
}

public enum Neo4jQueryOperationType
{
    InsertNode = 1,
    InsertNodes = 2,
    UpdateNode = 3,
    DeleteNodesAndRelations = 4,
    InsertRelation = 5,
    UpdateRelation = 6,
    DeleteRelations = 7,
    InsertLeaf = 8,
    DeleteLeaf = 9,
    FindNode = 10,
    FindRelation = 11,
}

public enum CommandType
{
    Add,
    Edit,
    Remove,
    Merge
}