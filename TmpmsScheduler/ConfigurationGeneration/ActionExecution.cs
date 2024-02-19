using Tmpms;

namespace TmpmsChecker.ConfigurationGeneration;

internal record ActionExecution(IEnumerable<ConsumeProduceSet> Moves)
{
    public virtual bool Equals(ActionExecution? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Moves.SequenceEqual(other.Moves);
    }

    public override int GetHashCode()
    {
        return Moves.GetHashCode();
    }
    
    public (Dictionary<string, IEnumerable<Part>> Consume, Dictionary<string, IEnumerable<Part>> Produce ) ToPartDictionary()
    {
        return (Moves.Select(e => e.Consume).ToDictionary(e => e.First().PartType),
                Moves.Select(e => e.Produce).ToDictionary(e => e.First().PartType));
    }
};