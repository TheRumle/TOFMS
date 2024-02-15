namespace Common.Errors;


public static class Errors
{
    public static Error AlreadyExists<T>()
    {
        return new Error(typeof(T).Name, "The is already registered for that context");
    }

    public static Error CouldNotFindSolution(string problemName)
    {
        return new Error("NoSolution", $"Could not find solution to {problemName}");
    }
    
}