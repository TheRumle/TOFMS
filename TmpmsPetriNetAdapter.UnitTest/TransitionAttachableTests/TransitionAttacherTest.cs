using Common;
using JsonFixtures;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using TestDataGenerator;
using Tmpms;
using Tmpms.Journey;
using Tmpms.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public abstract class TransitionAttacherTest : IClassFixture<MoveActionFixture>
{
    
    protected const string PartType = "P1";
    private int LocationId = 0;
    
    public HashSet<KeyValuePair<string, int>> PartsToMove = new()
    {
        new(PartType, 4)
    };
    
    public TransitionAttacherTest()
    {
        
    }

    public LocationGenerator CreateLocationGenerator(IEnumerable<string> partTypes)
    {
        return new LocationGenerator(partTypes);
    }



    protected Location CreateLocation(bool isProcessingLocation)
    {
        LocationId += 1;
        return new Location("Test location " + LocationId, 4, new[]
        {
            new Invariant(PartType, 0, Infteger.PositiveInfinity)
        }, isProcessingLocation, [PartType]);
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
            EmptyAfter = emptyAfter,
            EmptyBefore = emptyBefore,
            From = from,
            To = to,
            PartsToMove = PartsToMove
        };
    }
    
    protected MoveAction CreateMoveAction(Location from, Location to, HashSet<Location> emptyBefore, HashSet<Location> emptyAfter, int amount)
    {
        return new MoveAction()
        {
            Name = "Test",
            EmptyAfter = emptyAfter,
            EmptyBefore = emptyBefore,
            From = from,
            To = to,
            PartsToMove = new HashSet<KeyValuePair<string, int>>
            {
                new(PartType, amount)
            }
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

    protected Transition CreateTransition(JourneyCollection collection)
    {
        ColourTypeFactory factory = new ColourTypeFactory([PartType], collection);
        return new Transition("Test", factory.Transitions, TransitionGuard.Empty);
    }

    
}