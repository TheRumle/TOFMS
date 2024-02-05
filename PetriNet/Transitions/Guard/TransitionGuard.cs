using System.Reflection.Metadata.Ecma335;

namespace TACPN.Transitions.Guard;

public class And<T>(T first, T second)
{
    public readonly T First = first;
    public readonly T Second = second;
    public override string ToString()
    {
        return $"({first!.ToString()} AND {second!.ToString()})";
    }
}

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
        if (OrStatements.Count == 2) return  $"({OrStatements.First().ToTapaalText()} AND {OrStatements.Skip(1).First().ToTapaalText()})";
        if (OrStatements.Count == 3) return  $"({OrStatements.First().ToTapaalText()} AND {OrStatements.Skip(1).First().ToTapaalText()}) AND {OrStatements.Skip(2).First().ToTapaalText()})";

        var chunks = OrStatements.Chunk(2).ToArray();
        var last = chunks.Last();
        var endText = " AND " + (last.Length == 1 ? last[0].ToTapaalText() : new And<IOrStatement>(last[0], last[1]).ToString());


        var ands = chunks.SkipLast(1).Select(chunk => new And<IOrStatement>(chunk[0], chunk[1])).ToArray();
        string result = string.Join(" AND ", ands.Select(and => and.ToString()))
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