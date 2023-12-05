using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class OutGoingArc : Arc<Transition, IPlace>
{
    public IEnumerable<Production> Productions { get; set; }

    public OutGoingArc(Transition from, IPlace to, IEnumerable<Production> productions) : base(from, to)
    {
        Productions = productions;
    }
}