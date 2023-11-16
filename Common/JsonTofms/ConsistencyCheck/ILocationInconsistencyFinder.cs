namespace Common.JsonTofms.ConsistencyCheck;

public interface ILocationInconsistencyFinder
{
    public Task<ConsistencyCheckContext> FindInconsistencies(IEnumerable<Location> locations);

}