using Tmpms;

namespace TmpmsChecker.ConfigurationGeneration;

/// <summary>
/// Represents the consumption and production of a part type. 
/// </summary>
/// <param name="Consume"></param>
/// <param name="Produce"></param>
internal record ConsumeProduceSet(IEnumerable<Part> Consume, IEnumerable<Part> Produce)
{
    public virtual bool Equals(ConsumeProduceSet? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Consume.SequenceEqual(other.Consume) && Produce.SequenceEqual(other.Produce);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Consume, Produce);
    }

    public static ConsumeProduceSet ConstructWithJourneyUpdate(IEnumerable<Part> consume)
    {
        Part[] enumerable = consume as Part[] ?? consume.ToArray();
        return new ConsumeProduceSet(enumerable,enumerable.Select(e => new Part(e.PartType, 0, e.Journey.Skip(1))));
    }
    
    public static ConsumeProduceSet Construct(IEnumerable<Part> consume)
    {
        Part[] enumerable = consume as Part[] ?? consume.ToArray();
        return new ConsumeProduceSet(enumerable,enumerable.Select(e => e with { Age = 0 }));
    }
    
}