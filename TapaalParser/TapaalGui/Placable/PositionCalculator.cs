namespace TapaalParser.TapaalGui.Placable;

public static class PositionCalculator
{
    public static Position FindMidPoint(IEnumerable<Position> positions)
    {
        double sumX = 0;
        double sumY = 0;

        var enumerable = positions as Position[] ?? positions.ToArray();
        foreach (var position in enumerable)
        {
            sumX += position.X;
            sumY += position.Y;
        }

        var midpointX = sumX / enumerable.Length;
        var midpointY = sumY / enumerable.Length;

        return new Position(midpointX, midpointY);
    }
    
    public static Position[] GetCircularCoordinates(double centerX, double centerY, double radius, int N)
    {
        var coordinates = new Tuple<double, double>[N];

        for (int i = 0; i < N; i++)
        {
            double theta = (2 * Math.PI * i) / N;
            double x = centerX + radius * Math.Cos(theta);
            double y = centerY + radius * Math.Sin(theta);
            coordinates[i] = Tuple.Create(x, y);
        }

        return coordinates.Select(e=>new Position(e.Item1,e.Item2)).ToArray();
    }
}