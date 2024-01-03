using Common.UnitTests.Factories.fixtures;
using FluentAssertions;
using FluentAssertions.Execution;
using Tmpms.Common.Factories;
using Tmpms.Common.JsonTofms.Models;

namespace Common.UnitTests.Factories;

public class MoveActionFactoryTest : IClassFixture<TofmSystems>
{
    private readonly MoveActionFactory _factory = new();
    private readonly TofmJsonSystem _jsonSystemWithSameActions;


    public MoveActionFactoryTest(TofmSystems systems)
    {
        _jsonSystemWithSameActions = systems.SameActionSystem();
    }

    [Fact]
    public void WhenInputSystemHasActionsWithSameLocation_TheyGetReferenceToSameLocation()
    {
        var moveActions = _factory.CreateMoveActions(_jsonSystemWithSameActions);
        moveActions.Should().NotBeEmpty();
        var first = moveActions.ElementAt(0);
        var second = moveActions.ElementAt(1);

        using (new AssertionScope())
        {
            first.From.Should().BeSameAs(second.From);
            first.To.Should().BeSameAs(second.To);
            first.EmptyAfter.Should().BeEquivalentTo(second.EmptyAfter);
            first.EmptyBefore.Should().BeEquivalentTo(second.EmptyBefore);
            first.PartsToMove.Should().BeEquivalentTo(second.PartsToMove);
        }
    }
}