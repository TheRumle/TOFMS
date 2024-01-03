namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

public class MissingDefinitionError : InvalidJsonTofmException
{
    private readonly string _parent;
    private readonly string _fieldName;

    public MissingDefinitionError(string parent, string fieldName)
    {
        _parent = parent;
        _fieldName = fieldName;
    }
    
    public override string ErrorCategory { get; } = "Missing information";
    public override string ToString()
    {
        return $"Malformed json structure: {_parent}.{_fieldName} is formatted wrong. (Maybe undefined?)\n" ;
    }
}