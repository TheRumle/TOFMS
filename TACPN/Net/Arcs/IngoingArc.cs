using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class IngoingArc : Arc<Place, Transition>
{
    public IngoingArc(Place from, Transition to, IEnumerable<ColoredGuard> guards) : base(from, to)
    {
        Guards = guards.ToList();
    }

    public IList<ColoredGuard> Guards { get; set; }

    /// <summary>
    /// Replaces the existing guard with inputted guards color with the new guard.
    /// </summary>
    /// <param name="newGuard"></param>
    /// <returns>False if no matching guard of newGuard.Color exists, true otherwise</returns>
    public bool ReplaceGuard(ColoredGuard newGuard)
    {
        var oldGuard = Guards.FirstOrDefault(e => e.Color == newGuard.Color);
        if (oldGuard is null) return false;
        
        Guards.Remove(oldGuard);
        Guards.Add(newGuard);
        return true;
    }
}