﻿using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Database;

public abstract class DatabaseQueryableQueryObject<TObject> : QueryObject<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
    }

    protected DatabaseQueryContext Context
    {
        get;
    }

    protected DatabaseQueryResult QueryResult
    {
        get;
    }

    protected DatabaseQueryableQueryObject(ICacheInfoCollection cacheInfoCollection)
    {
        QueryResult = new();
        Context = new DatabaseQueryContext(cacheInfoCollection);
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override DatabaseQueryResult Build(IQuery<TObject> query)
    {
        switch (query.QueryOperationType)
        {
            case QueryOperationType.GetData:
                GenerateSelect(query);
                break;
            //case QueryOperationType.GetCount:
            //    GenerateCount(query);
            //    break;
            //case QueryOperationType.Add:
            //    GenerateInsert(query);
            //    break;
            //case QueryOperationType.Edit:
            //    GenerateUpdate(query);
            //    break;
            //case QueryOperationType.Remove:
            //    GenerateDelete(query);
            //    break;
        }

        return QueryResult;
    }

    //protected abstract void GenerateInsert(IQueryObject<TObject> query);

    //protected abstract void GenerateUpdate(IQueryObject<TObject> query);

    //protected abstract void GenerateDelete(IQueryObject<TObject> query);

    protected abstract void GenerateSelect(IQuery<TObject> query);

//    protected abstract void GenerateCount(IQueryObject<TObject> query);
}
