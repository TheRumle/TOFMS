using Common.Move;

namespace TACPN.Net.Arcs;

public class Arc<TFrom, TTo>
{

    public CountCollection<string> Amounts { get; protected set; } = new CountCollection<string>();

    public Arc(TFrom from, TTo to)
    {
        From = from;
        To = to;
    }

    public TFrom From { get; }
    public TTo To { get; }
}