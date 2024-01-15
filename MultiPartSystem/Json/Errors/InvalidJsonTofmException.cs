namespace Tmpms.Common.Json.Errors;

public abstract class InvalidJsonTofmException : Exception
{
    public InvalidJsonTofmException()
    {
    }
    
    public abstract string ErrorCategory { get; }
    public abstract override string ToString();
}