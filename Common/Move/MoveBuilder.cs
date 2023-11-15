namespace Common.Move;

public static class MoveBuilder
{
    /// <summary>
    /// Builds a move action with empty EmptyBefore or EmptyAfter sets.
    /// </summary>
    /// <param name="parts">The part type and how many of them to move</param>
    /// <returns>A MoveAction representing the movement part types and amounts defined in <paramref name="parts"/> </returns>
    public static MoveAction BuildSinglePartTypeAction(string name, Location from, Location to, KeyValuePair<string, int> parts )
    {
        return new MoveAction(name, from, to, new[] {parts });
    }
    
    public static MoveAction BuildSinglePartTypeAction(string name, Location from, Location to, KeyValuePair<string, int> parts, IEnumerable<Location> emptyBef)
    {
        return new MoveAction(name, from, to, new[] {parts }, emptyBef, new List<Location>());
    }
    
    public static MoveAction BuildSinglePartTypeAction(string name, Location from, Location to, KeyValuePair<string, int> parts, IEnumerable<Location> emptyBef,  IEnumerable<Location> emptyAfter)
    {
        return new MoveAction(name, from, to, new[] {parts }, emptyBef, emptyAfter);
    }
}