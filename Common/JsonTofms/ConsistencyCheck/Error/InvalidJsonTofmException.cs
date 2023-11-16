namespace Common.JsonTofms.ConsistencyCheck.Error;

public abstract class InvalidJsonTofmException : Exception
{
    public abstract override string ToString();

    public InvalidJsonTofmException()
    {
        
    }
    
    public InvalidJsonTofmException(string message) : base(message)
    {
        
    }

    public abstract string ErrorCategory { get; }

}