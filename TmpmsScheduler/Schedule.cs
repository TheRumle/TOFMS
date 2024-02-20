using TmpmsChecker.Algorithm;

namespace TmpmsChecker;
public class Schedule
{
    public readonly int TotalMakespan;
    
    public Schedule(IEnumerable<ReachableConfig> reachedStates)
    {
        TotalMakespan = reachedStates.Aggregate(0, (length, e) => length + e.TimeCost);
    }
}