using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.Models;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class JsonTofmParser
{
    private readonly ILocationInconsistencyFinder _locationInconsistencyFinder;
    private readonly IMoveActionLocationConsistencyEnsurer _locationConsistencyEnsurer;

    public JsonTofmParser(ILocationInconsistencyFinder locationInconsistencyFinder, IMoveActionLocationConsistencyEnsurer locationConsistencyEnsurer)
    {
        _locationInconsistencyFinder = locationInconsistencyFinder;
        _locationConsistencyEnsurer = locationConsistencyEnsurer;
    }

    public async Task<IEnumerable<MoveActionStructure>> ParseAsync(string jsonString)
    {
        TofmComponent? input = JsonConvert.DeserializeObject<TofmComponent>(jsonString);
        if (input is null) throw new ArgumentException($"The inputted string is not of the same format as {typeof(TofmComponent).FullName}.");
        
        ConsistencyCheckContext context = await _locationInconsistencyFinder.FindInconsistencies(input.Locations);
        if (context.Errors.Any())
            throw new ArgumentOutOfRangeException(context.ToErrorString());
        
        await _locationConsistencyEnsurer.RearrangeLocations(input.Moves);
        return input.Moves;
    }



}