namespace Common.JsonTofms.Error;

public abstract class LocationInconsistencyException : Exception
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