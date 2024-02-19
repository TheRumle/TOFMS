

namespace Tmpms;

public class Part(string PartType, int Age, IEnumerable<Location> Journey)
{
    protected bool Equals(Part other)
    {
        return PartType == other.PartType && Age == other.Age && Journey.Count() == other.Journey.Count();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Part)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PartType, Age, Journey);
    }

    public string PartType { get; init; } = PartType;
    public int Age { get; set; } = Age;
    public IEnumerable<Location> Journey { get; init; } = Journey;

    public void Deconstruct(out string PartType, out int Age, out IEnumerable<Location> Journey)
    {
        PartType = this.PartType;
        Age = this.Age;
        Journey = this.Journey;
    }
}