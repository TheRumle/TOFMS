namespace Tmpms.Json.Errors;

public class PartTypeNameEmptyException<T> : InvalidJsonTmpmsException where T : class
{
    public PartTypeNameEmptyException(T context)
    {
        Context = context;
    }

    public T Context { get; }

    public override string ErrorCategory { get; } = "Empty part type";

    public override string ToString()
    {
        return $"\"{Context.GetType().Name}\" did not specify a part type!";
    }
}