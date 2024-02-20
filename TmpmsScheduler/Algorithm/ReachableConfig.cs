using Tmpms.Move;

namespace TmpmsChecker.Algorithm;

public class ReachableConfig
{
    public readonly MoveAction? Action;
    public readonly bool ReachableByDelay;
    public readonly int TimeCost;
    public readonly Configuration ReachedConfiguration;
    
    public ReachableConfig(MoveAction move, Configuration configuration)
    {
        this.Action = move;
        this.ReachableByDelay = false;
        this.TimeCost = 0;
        ReachedConfiguration = configuration;
    }

    
    public ReachableConfig(int timeCost, Configuration configuration)
    {
        this.ReachableByDelay = true;
        this.TimeCost = timeCost;
        ReachedConfiguration = configuration;
    }


    public static ReachableConfig ZeroDelay(Configuration configuration)
    {
        return new ReachableConfig(0, configuration);
    }
}