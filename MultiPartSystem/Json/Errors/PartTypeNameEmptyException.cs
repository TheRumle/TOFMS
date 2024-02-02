namespace Tmpms.Common.Json.Errors;

public class PartTypeNameEmptyException<T> : InvalidJsonTmpmsException
{
    public PartTypeNameEmptyException(T context)
    {
        Context = context;
    }

    public T Context { get; }

    public override string ErrorCategory { get; } = "Empty part type";

    public override string ToString()
    {
        return $"\"{Context}\" did not specify a part type!";
    }
}