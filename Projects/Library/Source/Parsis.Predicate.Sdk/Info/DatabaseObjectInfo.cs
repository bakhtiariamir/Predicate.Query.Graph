using System;
using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Info;

public abstract class DatabaseObjectInfo<TObject> : ObjectInfo<TObject>, IDatabaseObjectInfo<TObject> where TObject : class
{
    public string Table
    {
        get;
        init;
    }

    public string Schema
    {
        get;
        init;
    } = "dbo";
}