using Tmpms.Json.Models;

namespace Tmpms.Json.Errors;

public abstract class LocationInconsistencyException : InvalidJsonTmpmsException
{
    public LocationInconsistencyException(LocationDefinition location)
    {
        First = location;
    }

    public LocationDefinition First { get; set; }

    public abstract override string ToString();
}