namespace TACPN.Transitions.Guard;


public interface ITransitionGuard
{
    public IReadOnlyList<IOrStatement> OrStatements { get; }
    public string ToTapaalText();

}

/// <summary>
/// Represents a series of AND conditions that must be true.
/// A guard contains multiple statements, where each statement contains conditions that are OR'd.
/// </summary>
public class TransitionGuard : ITransitionGuard
{
    public const string GUARDSEPARATOR = "AND";
    
    private class And<T>(T first, T second)
    {
        public override string ToString()
        {
            return $"({first!.ToString()} {GUARDSEPARATOR} {second!.ToString()})";
        }
    }
    
    private List<IOrStatement> _statements = new();
    public IReadOnlyList<IOrStatement> OrStatements => _statements.AsReadOnly();
    
    public void AddCondition(IOrStatement statement)
    {
        _statements.Add(statement);
    }
    private TransitionGuard(){}
    public static ITransitionGuard Empty() => new TransitionGuard();
    
    public string ToTapaalText()
    {
        if (OrStatements.Count == 1) return $"{OrStatements.First().ToTapaalText()}";
        if (OrStatements.Count == 2) return  $"({OrStatements.First().ToTapaalText()} {GUARDSEPARATOR} {OrStatements.Skip(1).First().ToTapaalText()})";
        if (OrStatements.Count == 3) return  $"({OrStatements.First().ToTapaalText()} {GUARDSEPARATOR} {OrStatements.Skip(1).First().ToTapaalText()}) {GUARDSEPARATOR} {OrStatements.Skip(2).First().ToTapaalText()})";

        var chunks = OrStatements.Chunk(2).ToArray();
        var last = chunks.Last();
        var endText = $" {GUARDSEPARATOR} " + (last.Length == 1 ? last[0].ToTapaalText() : new And<IOrStatement>(last[0], last[1]).ToString());


        var ands = chunks.SkipLast(1).Select(chunk => new And<IOrStatement>(chunk[0], chunk[1])).ToArray();
        string result = string.Join($" {GUARDSEPARATOR} ", ands.Select(and => and.ToString()))
            + endText;


        return result;
    }


    public override string ToString()
    {
        return this.ToTapaalText();
    }


    public static TransitionGuard FromAndedOrs(IEnumerable<IOrStatement> statements)
    {
        return new TransitionGuard()
        {
            _statements = statements.ToList()
        };
    }

}