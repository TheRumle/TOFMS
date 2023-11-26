namespace Tofms.Common.JsonTofms.ConsistencyCheck.Error;

public class LocationNameEmptyException<T> : InvalidJsonTofmException
{
    public LocationNameEmptyException(T context)
    {
        Context = context;
    }

    public T Context { get; }

    public override string ErrorCategory { get; } = "Empty location name";

    public override string ToString()
    {
        return $"{Context} had an empty location name!";
    }
}