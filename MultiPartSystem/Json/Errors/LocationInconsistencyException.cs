using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Errors;

public abstract class LocationInconsistencyException : InvalidJsonTmpmsException
{
    public LocationInconsistencyException(LocationDefinition location)
    {
        First = location;
    }

    public LocationDefinition First { get; set; }

    public abstract override string ToString();
}