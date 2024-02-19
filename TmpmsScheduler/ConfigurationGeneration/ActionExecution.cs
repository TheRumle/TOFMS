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
};