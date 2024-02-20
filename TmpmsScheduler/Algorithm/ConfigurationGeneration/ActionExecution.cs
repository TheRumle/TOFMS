using Tmpms;

namespace TmpmsChecker.Algorithm.ConfigurationGeneration;

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
        var allProdEmpty = Moves
            .Select(e => e.Produce).All(e => e.Any());
        
        var allConsEmpty = Moves
            .Select(e => e.Consume).All(e => e.Any());
        
        var prod = Moves.Select(e => e.Produce).ToArray();
        var cons = Moves.Select(e => e.Consume).ToArray();
        
        return  (allProdEmpty ? [] : cons.ToDictionary(e => e.First().PartType),
            allConsEmpty ? [] : prod.ToDictionary(e => e.First().PartType));
    }
};