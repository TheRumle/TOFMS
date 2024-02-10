using Common;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public abstract class TransitionAttacherTest : IClassFixture<MoveActionFixture>
{
    protected const string PartType = "P1";
    protected ColourType PartColourType;
    public readonly Transition Transition;
    public HashSet<KeyValuePair<string, int>> PartsToMove = new()
    {
        new(PartType, 4)
    };

    protected static Location CreateLocation(bool isProcessingLocation)
    {
        return new Location("TestToLocation", 4, new[]
        {
            new Invariant(PartType, 0, Infteger.PositiveInfinity)
        }, isProcessingLocation);
    }

    public TransitionAttacherTest(MoveActionFixture moveActionFixture)
    {
        this.Transition = new Transition("Test", PartColourType, TransitionGuard.Empty());
        this.PartColourType = moveActionFixture.PartColourType;
    }

    
    protected ColourTypeFactory CreateColourTypeFactory(JourneyCollection journey)
    {
        return new ColourTypeFactory([PartType], journey);
    }

    protected JourneyCollection SingletonJourney(Location nextStep)
    {
        return JourneyCollection.ConstructJourneysFor([(PartType, [nextStep])]);
    }

    protected MoveAction CreateMoveAction(Location from, Location to, HashSet<Location> emptyBefore, HashSet<Location> emptyAfter)
    {
        return new MoveAction()
        {
            Name = "Test",
            EmptyAfter = emptyBefore,
            EmptyBefore = emptyAfter,
            From = from,
            To = to,
            PartsToMove = PartsToMove
        };
    }
    
    protected MoveAction CreateMoveAction(Location from, Location to)
    {
        return new MoveAction()
        {
            Name = "Test",
            From = from,
            To = to,
            PartsToMove = PartsToMove
        };
    }
    
    
}