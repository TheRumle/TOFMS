namespace Common.Move;

public static class MoveBuilder
{
    /// <summary>
    ///     Builds a move action with empty EmptyBefore or EmptyAfter sets.
    /// </summary>
    /// <param name="parts">The part type and how many of them to move</param>
    /// <returns>A MoveAction representing the movement part types and amounts defined in <paramref name="parts" /> </returns>
    public static MoveAction BuildSinglePartTypeAction(string name, Location from, Location to,
        KeyValuePair<string, int> parts)
    {
        return Create(name, from, to, new[] { parts }, new List<Location>(), new List<Location>());
    }

    public static MoveAction BuildSinglePartTypeAction(string name, Location from, Location to,
        KeyValuePair<string, int> parts, IEnumerable<Location> emptyBef)
    {
        return Create(name, from, to, new[] { parts }, emptyBef, new List<Location>());
    }

    public static MoveAction BuildSinglePartTypeAction(string name, Location from, Location to,
        KeyValuePair<string, int> parts, IEnumerable<Location> emptyBef, IEnumerable<Location> emptyAfter)
    {
        return Create(name, from, to, new[] { parts }, emptyBef, emptyAfter);
    }

    public static MoveAction Create(string name, Location from, Location to,
        IEnumerable<KeyValuePair<string, int>> pairs,
        IEnumerable<Location> emptyBefore, IEnumerable<Location> emptyAfter)
    {
        return new MoveAction
        {
            To = to,
            From = from,
            PartsToMove = new HashSet<KeyValuePair<string, int>>(pairs),
            EmptyBefore = new HashSet<Location>(emptyBefore),
            EmptyAfter = new HashSet<Location>(emptyAfter)
        };
    }
}