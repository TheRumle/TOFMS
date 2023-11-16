
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck;

public class LocationInconsistencyFinder : ILocationInconsistencyFinder
{
    public Task<ConsistencyCheckContext> FindInconsistencies(IEnumerable<LocationStructure> locations)
    {
        throw new NotImplementedException();
    }
}