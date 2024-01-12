namespace TACPN.Arcs;

public class Arc<TFrom, TTo>
{
    public Arc(TFrom from, TTo to)
    {
        From = from;
        To = to;
    }

    public TFrom From { get; }
    public TTo To { get; }

    public override string ToString()
    {
        return $"{From} -> {To}";
    }
}