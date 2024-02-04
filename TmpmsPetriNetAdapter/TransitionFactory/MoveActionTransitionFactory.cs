using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;
using Tmpms.Common;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter.TransitionFactory;

public class MoveActionTransitionFactory : ITransitionFactory
{
    private readonly IndexedJourneyCollection _journeys;
    private readonly ColourVariable[] _variables;

    public MoveActionTransitionFactory(IndexedJourneyCollection journeysForParts, ColourVariable[] variables)
    {
        this._journeys = journeysForParts;
        this._variables = variables;
    }
    
    public Transition CreateTransition(MoveAction moveAction)
    {
        var relevantPartTypes = moveAction.PartsToMove.Select(e=>e.Key);
        
        IEnumerable<IColourComparison<ColourVariable,int>> comparisons = relevantPartTypes
            .SelectMany(partTypes => CreateEqualityComparisons(moveAction.To, partTypes));

        var a = TransitionGuardStatement.WithConditions(comparisons.ToList());
        var guard = TransitionGuard.FromStatement([a]);
        
        var t = new Transition(moveAction.Name, ColourType.TokenAndDefaultColourType, guard);
        return t;
    }

    private IEnumerable<VariableComparison> CreateEqualityComparisons(Location location, string partType)
    {
        IEnumerable<int> indexesRepresentingTo =  _journeys[partType]
            .Where(e=>e.Value.Equals(location))
            .Select(e=>e.Key);

        ColourVariable variableForPart = _variables.First(e => ColourVariable.VariableNameFor(partType) == e.Name);
        return indexesRepresentingTo.Select(index => VariableComparison.Equality(variableForPart, index));
    }

}