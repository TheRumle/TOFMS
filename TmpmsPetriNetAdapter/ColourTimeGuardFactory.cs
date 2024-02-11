using TACPN;
using TACPN.Colours.Type;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter;

public class ColourTimeGuardFactory
{
    private readonly ColourTypeFactory _ctFactory;

    public ColourTimeGuardFactory(ColourTypeFactory factory)
    {
        _ctFactory = factory;
    }
    
    public ColourTimeGuard CapacityGuard()
    {
        return ColourTimeGuard.Default();
    }

    public ColourTimeGuard TokensGuard(int from, int to)
    {
        return new ColourTimeGuard(_ctFactory.Tokens, new Interval(from,to));
    }
}