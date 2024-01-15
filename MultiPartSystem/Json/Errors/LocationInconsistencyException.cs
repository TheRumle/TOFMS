using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json.Errors;

public abstract class LocationInconsistencyException : InvalidJsonTofmException
{
    public LocationInconsistencyException(LocationDefinition location)
    {
        First = location;
    }

    public LocationDefinition First { get; set; }

    public abstract override string ToString();
}