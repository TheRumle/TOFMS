using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.Models;
using Common.Move;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class JsonParser
{
    private readonly ILocationInconsistencyFinder _locationInconsistencyFinder;
    private readonly IMoveActionLocationConsistencyEnsurer _locationConsistencyEnsurer;

    public JsonParser(ILocationInconsistencyFinder locationInconsistencyFinder, IMoveActionLocationConsistencyEnsurer locationConsistencyEnsurer)
    {
        _locationInconsistencyFinder = locationInconsistencyFinder;
        _locationConsistencyEnsurer = locationConsistencyEnsurer;
    }

    public async Task<IEnumerable<MoveAction>> ParseAsync(string jsonString)
    {
        Target? input = JsonConvert.DeserializeObject<Target>(jsonString);
        if (input is null) throw new ArgumentException($"The inputted string is not of the same format as {typeof(Target).FullName}.");
        
        ConsistencyCheckContext context = await _locationInconsistencyFinder.FindInconsistencies(input.Locations);
        if (context.Errors.Any())
            throw new ArgumentOutOfRangeException(context.ToErrorString());
        
        await _locationConsistencyEnsurer.RearrangeLocations(input.MoveActions);
        return input.MoveActions;
    }



}