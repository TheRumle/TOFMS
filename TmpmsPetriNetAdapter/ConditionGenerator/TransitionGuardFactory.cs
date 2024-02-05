using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter.ConditionGenerator;

public class TransitionGuardFactory
{
    private readonly Dictionary<string, IEnumerable<int>> _journeys;
    private readonly Location to;

    public TransitionGuardFactory(JourneyCollection collection, Location to )
    {
        this._journeys = collection.Select(e=>
                KeyValuePair.Create(e.Key, 
                    e.Value.Where(loc => loc.Equals(to)).Select((location, index) => index)))
            .ToDictionary();

        this.to = to;
    }

    public TransitionGuard MoveActionGuard(MoveAction moveAction)
    {
        IEnumerable<string> parts = moveAction.PartsToMove.Select(e => e.Key);
        parts.Select(e => CreateAllPossibleValues(e));
        Dictionary<string, IEnumerable<int>> partIntegersDict = GetJourneys();
        
        
        var indexes = this._journeys[parts.First()];











    }

    private IEnumerable<VariableComparison> CreateAllPossibleValues(string s)
    {
        var indexes = _journeys[s];
        indexes.Select(e=> VariableComparison)
        
        
        
        
        
        
        
        
        
        
        throw new NotImplementedException();
    }
}