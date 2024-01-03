using TACPN.Net.Arcs;
using TACPN.Net.Places;

namespace TACPN.Net.Transitions;

public static class TransitionExtension
{
    public static IngoingArc? GetArcFrom(this Transition transition, Places.Place from)
    {
        return transition.InGoing.FirstOrDefault(e => e.From == from);
    }

    public static Arc<Transition, IPlace>? GetArcTo(this Transition transition, Places.Place to)
    {
        return transition.OutGoing.FirstOrDefault(e => e.To == to);
    }

    public static IngoingArc? GetArcFrom(this Transition transition, string name)
    {
        return transition.InGoing.FirstOrDefault(e => e.From.Name == name);
    }

    public static Arc<Transition, IPlace>? GetArcTo(this Transition transition, string name)
    {
        return transition.OutGoing.FirstOrDefault(e => e.To.Name == name);
    }
    

    public static IEnumerable<CapacityPlace> CapacityPlaces(this Transition transition)
    {
        return transition.InvolvedPlaces.OfType<CapacityPlace>();
    }
}