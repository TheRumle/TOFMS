using Common.JsonTofms.Models;
using Common.Move;

namespace Common.JsonTofms.ConsistencyCheck;

public interface IMoveActionLocationConsistencyEnsurer
{
    /// <summary>
    /// Modifies the given MoveActions s.t they all share a reference to the same Locations if the locations have the same name.
    /// </summary>
    public Task RearrangeLocations(IEnumerable<MoveActionStructure> actions);
}