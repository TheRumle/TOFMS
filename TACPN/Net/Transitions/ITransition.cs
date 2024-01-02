using TACPN.Net.Arcs;
using TACPN.Net.Colours;

namespace TACPN.Net.Transitions;

public interface ITransition
{
    public ColourType Colour { get; }
    public string Name { get; }
    public ICollection<IngoingArc> InGoing { get; } 
    public ICollection<OutGoingArc> OutGoing { get; } 
    public ICollection<InhibitorArc> InhibitorArcs { get; }
    public Arc<Transition, IPlace> AddOutGoingTo(CapacityPlace from, Production production);
    
}