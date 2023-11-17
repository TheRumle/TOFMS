using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class OutGoingArc : Arc<Transition, Place>
{
    public readonly IEnumerable<Production> Productions;

    public OutGoingArc(Transition from, Place to, IEnumerable<Production> productions) : base(from, to)
    {
        Productions = productions;
    }
}