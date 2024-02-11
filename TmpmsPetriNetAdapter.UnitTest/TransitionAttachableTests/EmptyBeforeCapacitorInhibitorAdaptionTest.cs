using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures;
using TACPN.Transitions;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class EmptyBeforeCapacitorInhibitorAdaptionTest : TransitionAttacherTest
{
    public EmptyBeforeCapacitorInhibitorAdaptionTest(MoveActionFixture moveActionFixture) : base(moveActionFixture)
    {
    }
    
    [Fact]    
    public void ShouldHaveCorrectWeightFromCorrectArcs()
    {
        var emptyBef = new LocationGenerator([PartType])
            .Generate(50)
            .ToHashSet();
        var transition = CreateAndAttachToTransition(emptyBef);
        
            
        var emptyBeforeNames = emptyBef.Select(l => l.Name);
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().AllSatisfy(e => e.Weight.Should().Be(1));
            transition.InhibitorArcs.Should().AllSatisfy(e => emptyBeforeNames.Should().Contain(e.From.Name));
        }
    }
    
    protected Transition CreateAndAttachToTransition(HashSet<Location> emptyBef)
    {
        var journey = JourneyCollection.ConstructJourneysFor([(PartType, emptyBef)]);
        var transition = CreateTransition(journey);
        MoveAction action =
            CreateMoveAction(CreateLocation(true), CreateLocation(true), emptyBef, new HashSet<Location>());
        
        EmptyBeforeCapacitorInhibitorAdaption attacher = new EmptyBeforeCapacitorInhibitorAdaption(action, CreateColourTypeFactory(journey), journey);
        attacher.AttachToTransition(transition);
        return transition;
    }

}