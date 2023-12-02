using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class IngoingArc : Arc<Place, Transition>
{
    public IngoingArc(Place from, Transition to, IEnumerable<ColoredGuard> guards) : base(from, to)
    {
        Guards = guards.ToList();
    }

    public IList<ColoredGuard> Guards { get; set; }
}