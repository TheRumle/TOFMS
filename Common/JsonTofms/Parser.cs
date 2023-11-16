using Common.JsonTofms.ConsistencyCheck;
using Common.Move;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class Parser
{
    private readonly ILocationInconsistencyFinder _locationInconsistencyFinder;
    private readonly IMoveActionLocationConsistencyEnsurer _locationConsistencyEnsurer;

    public Parser(ILocationInconsistencyFinder locationInconsistencyFinder, IMoveActionLocationConsistencyEnsurer locationConsistencyEnsurer)
    {
        _locationInconsistencyFinder = locationInconsistencyFinder;
        _locationConsistencyEnsurer = locationConsistencyEnsurer;
    }

    public async Task<IEnumerable<MoveAction>> ParseAsync(string jsonString)
    {
        TOFMSJsonInput? input = JsonConvert.DeserializeObject<TOFMSJsonInput>(jsonString);
        if (input is null) throw new ArgumentException($"The inputted string is not of the same format as {typeof(TOFMSJsonInput).FullName}.");
        
        ConsistencyCheckContext context = await _locationInconsistencyFinder.FindInconsistencies(input.Locations);
        if (context.Errors.Any())
            throw new ArgumentOutOfRangeException(context.ToErrorString());
        
        await _locationConsistencyEnsurer.RearrangeLocations(input.MoveActions);
        return input.MoveActions;
    }



}