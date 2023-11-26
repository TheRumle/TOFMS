namespace Tofms.Common.JsonTofms.ConsistencyCheck.Error;

public abstract class InvalidJsonTofmException : Exception
{
    public InvalidJsonTofmException()
    {
    }

    public InvalidJsonTofmException(string message) : base(message)
    {
    }

    public abstract string ErrorCategory { get; }
    public abstract override string ToString();
}