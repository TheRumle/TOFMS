using TACPN.Net;
using TACPN.Net.Transitions;

namespace JsonFixtures.Tapn.Fixtures;

public class SimpleExampleFixture
{
    public PetriNetComponent Component { get; }


    public SimpleExampleFixture(PetriNetComponent component)
    {
        Component = component;
    }

    private static PetriNetComponent CreateComponent()
    {
        var colours = CreateColours();
        var places = CreatePlaces();
        var transitions = CreateTransition();
        
        return new PetriNetComponent
        {
            Colors = CreateColours(),
            Places = CreatePlaces(),
            Transitions = CreateTransition(),
        };
    }

    private static ICollection<Transition> CreateTransition()
    {
        throw new NotImplementedException();
    }

    private static ICollection<Place> CreatePlaces()
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<string> CreateColours()
    {
        throw new NotImplementedException();
    }
}