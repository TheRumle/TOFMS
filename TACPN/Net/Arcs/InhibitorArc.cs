using TACPN.Net.Transitions;
using Tofms.Common.Move;

namespace TACPN.Net.Arcs;

public class InhibitorArc : Arc<Place, Transition>
{
    public InhibitorArc(Place from, Transition to, int weight) : base(from, to)
    {
        Weight = weight;
    }


    public int Weight { get; set; }
}