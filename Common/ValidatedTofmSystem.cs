using Tmpms.Common.Move;

namespace Tmpms.Common;

public class ValidatedTofmSystem
{
    public Dictionary<string, List<Location>> Journeys { get; set; }
    public IEnumerable<MoveAction> MoveActions { get; set; }
    public IEnumerable<string> Parts { get; set; }
    
}