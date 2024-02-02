namespace Tmpms.Common.Json.Errors;

public abstract class InvalidJsonTmpmsException : Exception
{
    public InvalidJsonTmpmsException()
    {
    }
    
    public abstract string ErrorCategory { get; }
    public abstract override string ToString();
}