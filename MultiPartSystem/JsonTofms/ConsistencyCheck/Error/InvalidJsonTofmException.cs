namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

public abstract class InvalidJsonTofmException : Exception
{
    public InvalidJsonTofmException()
    {
    }
    
    public abstract string ErrorCategory { get; }
    public abstract override string ToString();
}