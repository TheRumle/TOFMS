using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.Algorithm;

public class ReachedState
{
    private readonly MoveAction? _move;
    private readonly int _delay;
    private readonly bool _hasDelay;
    public readonly Configuration ReachedConfiguration;

    public ReachedState(MoveAction move, Configuration configuration)
    {
        this._move = move;
        this._hasDelay = false;
        ReachedConfiguration = configuration;
    }

    
    public ReachedState(int delay, Configuration configuration)
    {
        this._hasDelay = true;
        this._delay = delay;
        ReachedConfiguration = configuration;
    }


    /// <summary>
    ///  Cost of executing an action is 0 cost of delay is equal to delay
    /// </summary>
    /// <returns></returns>
    public int ActionCost()
    {
        return _hasDelay ? _delay : 0;
    }

    public static ReachedState ZeroDelay(Configuration configuration)
    {
        return new ReachedState(0, configuration);
    }
}