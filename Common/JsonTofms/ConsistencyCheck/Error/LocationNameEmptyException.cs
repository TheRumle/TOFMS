namespace Common.JsonTofms.ConsistencyCheck.Error;

public class LocationNameEmptyException<T> : InvalidJsonTofmException
{

    public LocationNameEmptyException(T context)
    {
        this.Context = context;
    }

    public T Context { get; }

    public override string ToString()
    {
        return $"{Context} had an empty location name!";
    }

    public override string ErrorCategory { get; } = "Empty location name";
}

public class PartTypeNameEmptyException<T> : InvalidJsonTofmException
{

    public PartTypeNameEmptyException(T context)
    {
        this.Context = context;
    }

    public T Context { get; }

    public override string ToString()
    {
        return $"{Context} did not specify a part type!";
    }

    public override string ErrorCategory { get; } = "Empty part type";
}