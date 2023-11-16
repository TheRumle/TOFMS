namespace Common.JsonTofms.ConsistencyCheck.Error;

public abstract class LocationInconsistencyException : InvalidJsonTofmException
{
    public LocationInconsistencyException(Location firstLocation, Location secondLocation)
    {
        this.First = firstLocation;
        this.Second = secondLocation;
    }
    public Location Second { get; set; }
    public Location First { get; set; }

    public abstract override string ToString();
}