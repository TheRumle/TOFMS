namespace TACPN.Net;

public static class GuardFactory
{
    public static ColoredGuard CapacityGuard(int amount)    
    {
        return new ColoredGuard(amount, CapacityPlaceExtensions.DefaultCapacityColor, Interval.ZeroToInfinity);
    }
}