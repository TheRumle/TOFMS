using Common;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public abstract class TransitionAttacherTest
{
    protected const string PartType = "P1";

    public abstract ITransitionAttachable CreateFromLocation(Location location);

    protected static Location CreateLocation(bool isProcessingLocation)
    {
        return new Location("TestToLocation", 4, new[]
        {
            new Invariant(PartType, 0, Infteger.PositiveInfinity)
        }, isProcessingLocation);
    }


    public Transition GetTransition()
    {
        var t = new Transition("t", ColourType.TokenAndDefaultColourType, TransitionGuard.Empty());
        return t;
    }
    
    
    protected static Dictionary<string, IEnumerable<Location>> GetJourneys(Location toLocation)
    {
        var result = new Dictionary<string, IEnumerable<Location>>();
        result.Add(PartType, new List<Location>()
        {
            toLocation
        });
        return result;
    }
    
    protected (Transition transition, Location origin) CreateAndAttach(bool isProccesingLocation)
    {
        var location = CreateLocation(isProccesingLocation);
        var attachable = CreateFromLocation(location);
        var transition = GetTransition();
        attachable.AttachToTransition(transition);
        return (transition,location);
    }
    
    protected (Transition transition, Location origin) CreateAndAttach(ITransitionAttachable attachable, Location location)
    {
        var transition = GetTransition();
        attachable.AttachToTransition(transition);
        return (transition,location);
    }

    protected string GetExpectedVariableExpressionValue(bool isProcessingLocation)
    {
        var variableExpressionValue = isProcessingLocation
            ? ColourVariable.DecrementFor(PartType).Value
            : ColourVariable.VariableNameFor(PartType);
        return variableExpressionValue;
    }
}