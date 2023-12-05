using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class IngoingArc : Arc<IPlace, Transition>
{
    public IngoingArc(IPlace from, Transition to, IEnumerable<ColoredGuard> guards) : base(from, to)
    {
        Guards = guards.ToList();
    }

    public IList<ColoredGuard> Guards { get; set; }
    public bool ReplaceGuard(ColoredGuard newGuard)
    {
        var oldGuard = Guards.FirstOrDefault(e => e.Color == newGuard.Color);
        if (oldGuard is null) return false;
        
        Guards.Remove(oldGuard);
        Guards.Add(newGuard);
        return true;
    }
}