using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck;

public interface ILocationInconsistencyFinder
{
    public Task<ConsistencyCheckContext> FindInconsistencies(IEnumerable<LocationStructure> locations);

}