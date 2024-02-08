using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter.ConditionGenerator;

public class TransitionGuardFactory
{
    private readonly IndexedJourneyCollection _journeys;
    private readonly ColourType _partsColourType;

    public TransitionGuardFactory(JourneyCollection collection, ColourType partsColourType)
    {
        this._journeys = collection.ToIndexedJourney();
        this._partsColourType = partsColourType;
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
            
            var comparisonsForPart = CreateVariableComparisons(indexAndLocation, part, _journeys[part].Count());
            comparisons.AddRange(comparisonsForPart);;
        }

        IEnumerable<IOrStatement> statements = CreateOrs(comparisons);
        return TransitionGuard.FromAndedOrs(statements);
    }

    private IEnumerable<IOrStatement> CreateOrs(List<VariableComparison> comparisons)
    {
        var groupByPart = comparisons.GroupBy(e => e.Lhs.Name);
        return groupByPart.Select(e =>
            OrStatement.FromPartComparisons(e.Select(comparison => comparison), _partsColourType)
        );
    }

    private IEnumerable<VariableComparison> CreateVariableComparisons(IEnumerable<KeyValuePair<int, Location>> indexAndLocation, string part, int journeyLenght)
    {
        return indexAndLocation.Select(indexedLocation => new VariableComparison(ColourComparisonOperator.Eq,
            ColourVariable.CreateFromPartName(part,journeyLenght),
            indexedLocation.Key));
    }
}