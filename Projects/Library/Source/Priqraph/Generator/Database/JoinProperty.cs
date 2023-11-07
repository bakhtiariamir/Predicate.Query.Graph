using Priqraph.Contract;
using Priqraph.DataType;
using System;

namespace Priqraph.Generator.Database
{
    public class JoinProperty
    {
        public IColumnPropertyInfo JoinColumn
        {
            get;
        }

        public JoinType JoinType
        {
            get;
        }

        public IDatabaseObjectInfo JoinObjectInfo
        {
            get;
        }

        public JoinProperty(IColumnPropertyInfo joinColumn, JoinType joinType, IDatabaseObjectInfo joinObjectInfo)
        {
            JoinColumn = joinColumn;
            JoinType = joinType;
            JoinObjectInfo = joinObjectInfo;
        }
    }
}
