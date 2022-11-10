using System;

namespace Parsis.Predicate.Sdk.DataType;

public enum ConditionOperatorType
{
    Equal = 1,
    NotEqual = 2,
    Like = 3,
    NotLike = 4,
    Contain = 5,
    RightContain = 6,
    LeftContain = 7,
    In = 8,
    NotIn = 9,
    BiggerTha = 10,
    EqualBiggerThan = 11,
    LesserThan = 12,
    EqualLesserThan = 13,
    Between = 14
}
