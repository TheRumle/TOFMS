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


    public int ToInt()
    {
        return _hasDelay ? _delay : 0;
    }
    
}