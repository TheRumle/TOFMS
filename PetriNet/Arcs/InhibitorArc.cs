using TACPN.Places;
using TACPN.Transitions;

namespace TACPN.Arcs;

public class InhibitorArc : Arc<Place, Transition>
{
    public InhibitorArc(Place from, Transition to, int weight) : base(from, to)
    {
        Weight = weight;
    }


    public int Weight { get; set; }
}