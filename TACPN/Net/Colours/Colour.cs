namespace TACPN.Net;

public record struct Colour(string Value)
{
    public static implicit operator string(Colour color)
    {
        return color.Value;
    }
}