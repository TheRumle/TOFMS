namespace TapaalParser.TapaalGui.Old.Placable;

public record Position(int X, int Y)
{
    public Position(double x, double y): this((int)Math.Round(x), (int)Math.Round(y))
    {
    }
    
    
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