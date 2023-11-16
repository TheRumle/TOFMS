using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Error;

public class InconsistentCapacityException : LocationInconsistencyException
{
    public InconsistentCapacityException(LocationStructure firstLocation, int secondCapacity) : base(firstLocation)
    {
        this.FirstCapacity = firstLocation.Capacity;
        this.SecondCapacity = secondCapacity;
    }

    public int FirstCapacity { get; set; }

    public int SecondCapacity { get; set; }
    public override string ToString()
    {
        return
            $"Inconsistent capacity: Found multiple capacity definitions of {First.Name}: {FirstCapacity},{SecondCapacity}";
    }

    public override string ErrorCategory { get; } = "Multiple capacities defined";
}