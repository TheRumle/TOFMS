namespace TACPN.Transitions.Guard;

public interface ITransitionGuard
{
    public IReadOnlyList<ITransitionGuardStatement> Statements { get; }
    public string ToTapaalText();

}

/// <summary>
/// Represents a series of AND conditions that must be true.
/// A guard contains multiple statements, where each statement contains conditions that are OR'd.
/// </summary>
public class TransitionGuard : ITransitionGuard
{
    private List<ITransitionGuardStatement> _statements = new();
    public IReadOnlyList<ITransitionGuardStatement> Statements => _statements.AsReadOnly();
    
    public void AddCondition(ITransitionGuardStatement statement)
    {
        _statements.Add(statement);
    }
    private TransitionGuard(){}
    public static ITransitionGuard Empty() => new TransitionGuard();
    
    public string ToTapaalText()
    {
        
        //AddParanthesis
        throw new NotImplementedException();
    }


    public static TransitionGuard FromStatement(IEnumerable<ITransitionGuardStatement> statements)
    {
        return new TransitionGuard()
        {
            _statements = statements.ToList()
        };
    }

}