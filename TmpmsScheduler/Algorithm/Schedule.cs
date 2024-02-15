namespace TMPMSChecker.Algorithm;

public interface ISystemAction;

public class MachineAction : ISystemAction
{
    
}

public class Delay : ISystemAction
{
    
}

public class Schedule
{
    public readonly int TotalMakespan;
    
    public Schedule(IEnumerable<ReachedState> reachedStates)
    {
        
        TotalMakespan = reachedStates.Aggregate(0, (length, e) => length + e.ToValue());
    }
}