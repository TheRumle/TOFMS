using Common.Move;
using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class InhibitorArc : Arc<Place, Transition> 
{
    public CountCollection<string> Amounts { get; } = new();
    public InhibitorArc(Place from, Transition to, string color) : base(from, to)
    {
        this.Amounts.AddKey(color);
    }
    
    public InhibitorArc(Place from, Transition to, IEnumerable<string> colors) : base(from, to)
    {
        this.Amounts = new CountCollection<string>(colors);
    }
    
    public InhibitorArc(Place from, Transition to, IEnumerable<KeyValuePair<string,int>> values) : base(from, to)
    {
        this.Amounts = new CountCollection<string>(values);
    }
    public InhibitorArc(Place from, Transition to, KeyValuePair<string,int> values) : base(from, to)
    {
        this.Amounts = new CountCollection<string>(values);

    }
    
    
    
}