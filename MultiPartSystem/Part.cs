namespace Tmpms;

public record Part(string PartType, int Age, IEnumerable<Location> Journey)
{
    public virtual bool Equals(Part? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return PartType == other.PartType && Age == other.Age && Journey.Count() == other.Journey.Count();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PartType, Age, Journey);
    }
}