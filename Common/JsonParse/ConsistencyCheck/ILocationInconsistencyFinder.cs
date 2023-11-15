namespace Common.JsonParse.ConsistencyCheck;

public interface ILocationInconsistencyFinder
{
    public Task<ConsistencyCheckContext> FindInconsistencies(IEnumerable<Location> locations);

}