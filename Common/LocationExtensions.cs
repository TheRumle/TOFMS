namespace Common;

public static class LocationExtensions
{
    public static string CapacityName(this Location location)
    {
        return location.Name + "_buffer";
    }
}