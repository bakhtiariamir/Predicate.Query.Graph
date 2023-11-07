namespace Priqraph.Contract;
public interface IQueryFragment<out TParameter> : IQueryFragment where TParameter : class
{
    TParameter? Parameter
    {
        get;
    }
}

public interface IQueryFragment
{
    string? Text
    {
        get;
    }
}

