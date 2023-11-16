using System.Text;
using Common.JsonTofms.ConsistencyCheck.Error;

namespace Common.JsonTofms.ConsistencyCheck;

public class ConsistencyCheckContext
{
    public ConsistencyCheckContext(IEnumerable<LocationInconsistencyException> errors, IEnumerable<Location> okayLocations)
    {
        Errors = errors;
        OkayLocations = okayLocations;
    }

    public IEnumerable<LocationInconsistencyException> Errors { get; init; }
    public IEnumerable<Location> OkayLocations { get; init; }

    public string ToErrorString()
    {
        var bob = new StringBuilder("Found errors:\n");
        var errorsByLocationName = Errors.GroupBy(e => e.First.Name);
        foreach (var errGroup in errorsByLocationName)
        {
            bob.AppendLine("Errors for").Append(errGroup.Key).Append(": ").Append(errGroup);
            foreach (var exception in errGroup)
                bob.Append('\t').Append(exception);
        }

        return bob.ToString();
    }
}