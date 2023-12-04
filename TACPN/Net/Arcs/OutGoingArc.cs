using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class OutGoingArc : Arc<Transition, Place>
{
    public IEnumerable<Production> Productions { get; set; }

    public OutGoingArc(Transition from, Place to, IEnumerable<Production> productions) : base(from, to)
    {
        Productions = productions;
    }
}