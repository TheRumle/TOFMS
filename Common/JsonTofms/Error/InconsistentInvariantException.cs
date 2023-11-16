namespace Common.JsonTofms.Error;

public class InconsistentInvariantException : LocationInconsistencyException
{
    public InconsistentInvariantException(Location firstLocation, Invariant firstInvariant,  Location secondLocation, Invariant secondInvariant) : base(firstLocation, secondLocation)
    {
        this.FirstInvariant =  firstInvariant;
        this.SecondInvariant = secondInvariant;
    }

    public Invariant FirstInvariant { get; set; }

    public Invariant SecondInvariant { get; set; }

    public override string ToString()
    {
        return
            $"Inconsistent capacity: Found multiple capacity definitions of {First.Name}: {FirstInvariant},{SecondInvariant}";
    }
}