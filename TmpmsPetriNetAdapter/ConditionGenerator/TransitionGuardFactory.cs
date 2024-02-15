using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;
using Tmpms;
using Tmpms.Journey;
using Tmpms.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.ConditionGenerator;

public class TransitionGuardFactory
{
    private readonly IndexedJourneyCollection _journeys;
    private readonly ColourType _transitionColourType;
    private readonly ColourVariableFactory _variableFactory;

    public TransitionGuardFactory(ColourTypeFactory ctFactory, ColourVariableFactory variableFactory)
    {
        _journeys = ctFactory.JourneyCollection.ToIndexedJourney();
        _transitionColourType = ctFactory.Transitions;
        _variableFactory =variableFactory;
    }

    public TransitionGuard MoveActionGuard(MoveAction moveAction)
    {
        IEnumerable<string> parts = moveAction.PartsToMove.Select(e => e.Key);


        List<VariableComparison> comparisons = new List<VariableComparison>();
        foreach (var part in parts)
        {
            IEnumerable<KeyValuePair<int, Location>> indexAndLocation = 
                _journeys[part]
                .Where(kvp=> kvp.Value == moveAction.To);
            
            var comparisonsForPart = CreateVariableComparisons(indexAndLocation, part);
            comparisons.AddRange(comparisonsForPart);;
        }

        IEnumerable<IOrStatement> statements = CreateOrs(comparisons);
        return TransitionGuard.FromAndedOrs(statements);
    }

    private IEnumerable<IOrStatement> CreateOrs(List<VariableComparison> comparisons)
    {
        var groupByPart = comparisons.GroupBy(e => e.Lhs.Name);
        return groupByPart.Select(e =>
            OrStatement.FromPartComparisons(e.Select(comparison => comparison), _transitionColourType)
        );
    }

    private IEnumerable<VariableComparison> CreateVariableComparisons(IEnumerable<KeyValuePair<int, Location>> indexAndLocation, string part)
    {
        return indexAndLocation.Select(indexedLocation => new VariableComparison(ColourComparisonOperator.Eq,
            _variableFactory.VariableForPart(part),
            indexedLocation.Key));
    }
}