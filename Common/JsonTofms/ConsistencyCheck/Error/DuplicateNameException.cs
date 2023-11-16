namespace Common.JsonTofms.ConsistencyCheck.Error;

public class DuplicateNameException<TFirst,TSecond> : InvalidJsonTofmException
{
    private readonly TFirst _first;
    private readonly TSecond _second;
    private readonly string _name;

    public DuplicateNameException(TFirst first, TSecond second, string name)
    {
        _first = first;
        _second = second;
        _name = name;
    }
    
    
    public override string ToString()
    {
        return $"Found elements with duplicate name '{_name}': {_first} and {_second}";
    }

    public override string ErrorCategory { get; } = $"Duplicate name";
}

public class DuplicateNameException<TFirst,TSecond, TThird> : InvalidJsonTofmException
{
    private readonly TFirst _first;
    private readonly TSecond _second;
    private readonly string _name;
    private readonly TThird _third;

    public DuplicateNameException(TFirst first, TSecond second, TThird third, string name)
    {
        _first = first;
        _second = second;
        _third = third;
        _name = name;
    }
    
    
    public override string ToString()
    {
        return $"Found elements with duplicate name '{_name}': {_first}, {_second}, and {_third}";
    }

    public override string ErrorCategory { get; } = $"Duplicate name";
}