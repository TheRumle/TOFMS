using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Error;

public class InconsistentCapacityException : LocationInconsistencyException
{
    public InconsistentCapacityException(LocationDefinition firstLocation, int secondCapacity) : base(firstLocation)
    {
        FirstCapacity = firstLocation.Capacity;
        SecondCapacity = secondCapacity;
    }

    public int FirstCapacity { get; set; }

    public int SecondCapacity { get; set; }

    public override string ErrorCategory { get; } = "Multiple capacities defined";

    public override string ToString()
    {
        return
            $"Inconsistent capacity: Found multiple capacity definitions of {First.Name}: {FirstCapacity},{SecondCapacity}";
    }
}