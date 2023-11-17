using TACPN.Net.Arcs;

namespace TACPN.Net.Transitions;

public static class TransitionExtension
{
    public static IngoingArc? GetArcFrom(this Transition transition, Place from)
    {
        return transition.InGoing.FirstOrDefault(e => e.From == from);
    }

    public static Arc<Transition, Place>? GetArcTo(this Transition transition, Place to)
    {
        return transition.OutGoing.FirstOrDefault(e => e.To == to);
    }

    public static IngoingArc? GetArcFrom(this Transition transition, string name)
    {
        return transition.InGoing.FirstOrDefault(e => e.From.Name == name);
    }

    public static Arc<Transition, Place>? GetArcTo(this Transition transition, string name)
    {
        return transition.OutGoing.FirstOrDefault(e => e.To.Name == name);
    }
}