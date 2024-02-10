using Common;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public abstract class TransitionAttacherTest
{
    protected const string PartType = "P1";

    public abstract ITransitionAttachable CreateFromLocation(Location from, Location to);

    protected static Location CreateLocation(bool isProcessingLocation)
    {
        return new Location("TestToLocation", 4, new[]
        {
            new Invariant(PartType, 0, Infteger.PositiveInfinity)
        }, isProcessingLocation);
    }


    public Transition GetTransition()
    {
        var t = new Transition("t", TokenColourType, TransitionGuard.Empty());
        return t;
    }
    
    
    protected static JourneyCollection GetJourneys(Location toLocation)
    {
        var result = new JourneyCollection();
        result.Add(PartType, new List<Location>()
        {
            toLocation
        });
        return result;
    }
    
    protected (Transition transition, Location from) CreateAndAttach(bool isFromProcessing, bool isToProcessing = false)
    {
        var from = CreateLocation(isFromProcessing);
        var to = CreateLocation(isToProcessing);
        var attachable = CreateFromLocation(from, to);
        var transition = GetTransition();
        attachable.AttachToTransition(transition);
        return (transition,from);
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
            ? VariableFactory.DecrementForPart(PartType).Value
            : ColourVariable.VariableNameFor(PartType);
        return variableExpressionValue;
    }

    public TransitionAttacherTest()
    {
        this.ColourTypeFactory = new ColourTypeFactory([PartType], new ());
        this.VariableFactory = new ColourVariableFactory(ColourTypeFactory);
        
        PartColourType  = ColourTypeFactory.Parts;
        TokenColourType = ColourTypeFactory.Tokens;
    }

    public ColourVariableFactory VariableFactory { get; set; }

    public ColourTypeFactory ColourTypeFactory { get; }

    protected ColourType TokenColourType;
    protected ColourType PartColourType;
}