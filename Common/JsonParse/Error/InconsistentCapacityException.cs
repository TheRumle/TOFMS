namespace Common.JsonParse.Error;

public class InconsistentCapacityException : LocationInconsistencyException
{
    public InconsistentCapacityException(Location firstLocation, Location secondLocation) : base(firstLocation, secondLocation)
    {
        this.FirstCapacity = firstLocation.Capacity;
        this.SecondCapacity = secondLocation.Capacity;
    }

    public int FirstCapacity { get; set; }

    public int SecondCapacity { get; set; }
    public override string ToString()
    {
        return
            $"Inconsistent capacity: Found multiple capacity definitions of {First.Name}: {FirstCapacity},{SecondCapacity}";
    }
}