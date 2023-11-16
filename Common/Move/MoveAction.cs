namespace Common.Move;

public record MoveAction
{
    public MoveAction(string name,Location from,  Location to,  IEnumerable<string> partsToMove)
    {
        if (!name.IsAlphaNumericOnly()) throw new ArgumentException(nameof(name) + " must only be alpha numerical!");
        To = to;
        From = from;
        PartsToMove = new CountCollection<string>(partsToMove);
    }
    
    public MoveAction(string name,Location from,  Location to,  IEnumerable<KeyValuePair<string, int>> pairs)
    {
        if (!name.IsAlphaNumericOnly()) throw new ArgumentException(nameof(name) + " must only be alpha numerical!");
        To = to;
        From = from;
        PartsToMove = new CountCollection<string>(pairs);
        EmptyBefore = new HashSet<Location>();
        EmptyAfter= new HashSet<Location>();
    }

    public ISet<Location> EmptyAfter { get; init; }

    public ISet<Location> EmptyBefore { get; init; }

    public MoveAction(string name, Location from, Location to, IEnumerable<KeyValuePair<string, int>> pairs,
        IEnumerable<Location> emptyBefore, IEnumerable<Location> emptyAfter)
    {
        if (!name.IsAlphaNumericOnly()) throw new ArgumentException(nameof(name) + " must only be alpha numerical!");
        To = to;
        From = from;
        PartsToMove = new CountCollection<string>(pairs);
        EmptyBefore = new HashSet<Location>(emptyBefore);
        EmptyAfter = new HashSet<Location>(emptyAfter);
    }

    public string Name { get; init; }

    public Location To { get; init; }
    public Location From { get; init; }
    public IEnumerable<KeyValuePair<string, int>> PartsToMove { get; init; } = new CountCollection<string>();


}