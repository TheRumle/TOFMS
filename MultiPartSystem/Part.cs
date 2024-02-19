

namespace Tmpms;

public class Part(string PartType, int Age, IEnumerable<Location> Journey)
{
    public virtual bool Equals(Part? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return PartType == other.PartType && Age == other.Age && Journey.Equals(other.Journey);
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