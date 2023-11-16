using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Error;

public abstract class LocationInconsistencyException : InvalidJsonTofmException
{
    public LocationInconsistencyException(LocationStructure location)
    {
        this.First = location;
    }
    public LocationStructure First { get; set; }

    public abstract override string ToString();
}