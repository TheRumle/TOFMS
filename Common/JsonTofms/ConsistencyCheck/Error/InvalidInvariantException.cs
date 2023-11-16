namespace Common.JsonTofms.ConsistencyCheck.Error;

public class InvalidInvariantException : InvalidJsonTofmException
{
    private readonly string _part;
    private readonly string _min;
    private readonly string _max;
    private readonly string _readableMessage;

    public InvalidInvariantException(string part, string min, string max) : base( $"Invalid definition invariant for {part}: {min} to {max}. Is {min} > {max} and did you use infty/inf wrong?")
    {
        _part = part;
        _min = min;
        _max = max;
        this._readableMessage =  $"Invalid definition invariant for {_part}: {_min} to {_max}. Is {_min} > {_max} and did you use infty/inf wrong?";
    }

    public override string ToString()
    {
        return _readableMessage;
    }
}