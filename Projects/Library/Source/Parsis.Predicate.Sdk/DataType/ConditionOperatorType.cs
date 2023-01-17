using System;

namespace Parsis.Predicate.Sdk.DataType;

public enum ConditionOperatorType
{
    None = 0,
    Equal = 1,
    NotEqual = 2,
    Like = 3,
    NotLike = 4,
    RightLike = 5,
    NotRightLike = 6,
    LeftLike = 7,
    NotLeftLike = 8,
    In = 9,
    NotIn = 10,
    GreaterThan = 11,
    GreaterThanEqual = 12,
    LessThan = 13,
    LessThanEqual = 14,
    Between = 15,
    IsNull = 16,
    NotIsNull = 17,
    And = 18,
    Or = 19,
    Set = 20,
    Contains = 21
}
