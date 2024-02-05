using JsonFixtures;
using TACPN.Colours.Values;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionFactory;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionFactory;

public abstract class MoveActionDependentTest : IClassFixture<MoveActionFixture>
{
    public Func<IDictionary<string, int>, ColourVariable[]> CreateVariables { get; set; }
    public readonly Location BufferLocation;
    public readonly Location ProcessingLocation;
    protected readonly IEnumerable<Location> _otherLocations;
    
    public HashSet<KeyValuePair<string, int>> Move(params (int amount, string partType)[] parts)
    {
        return parts.Select(e => KeyValuePair.Create(e.partType, e.amount)).ToHashSet();
    }

    protected MoveAction CreateMoveAction(Location from, Location to, HashSet<KeyValuePair<string, int>> partsToMove)
    {
        return new MoveAction()
        {
            Name = "Test action",
            From = from,
            To = to,
            EmptyAfter = new HashSet<Location>(),
            EmptyBefore = new HashSet<Location>(),
            PartsToMove = partsToMove
        };
    }
    
        
    protected ITransitionFactory CreateTransitionFactoryForJourneys(JourneyCollection collection)
    {
        return new MoveActionTransitionFactory(collection.ToIndexedJourney(), CreateVariables(collection.JourneyLengths()));
    }

    public MoveActionDependentTest(MoveActionFixture fixture)
    {
        this.ProcessingLocation = fixture.ProcessingLocation;
        this.BufferLocation = fixture.BufferLocation;
        this.CreateVariables = MoveActionFixture.VariablesForParts;
        this._otherLocations = fixture.LocationGenerator.GenerateLocations(10);
    }
}