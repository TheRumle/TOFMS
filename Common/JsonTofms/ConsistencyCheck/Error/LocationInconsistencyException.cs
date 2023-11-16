using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Error;

public abstract class LocationInconsistencyException : InvalidJsonTofmException
{
    public LocationInconsistencyException(LocationDefinition location)
    {
        this.First = location;
    }
    public LocationDefinition First { get; set; }

    public abstract override string ToString();
}