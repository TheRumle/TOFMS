namespace TapaalParser.TapaalGui.Placable;

public record Position(double X, double Y)
{
    public static Position operator +(Position first, Position second)
    {
        return new Position(first.X + second.X, first.Y + second.Y);
    }
    
    public static Position operator -(Position first, Position second)
    {
        return new Position(first.X - second.X, first.Y - second.Y);
    }

    public static Position Zero = new Position(0, 0);


}