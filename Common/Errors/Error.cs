
namespace Common.Errors;

public sealed class Error(string errorName, string description)
{
    public static readonly Error NullError = new Error(nameof(NullError), "A value could not be obtained");
    public static readonly Error None = new Error(nameof(None), "The result could be achieved without errors");
    public string ErrorName { get; init; } = errorName;
    public string Description { get; init; } = description;
}