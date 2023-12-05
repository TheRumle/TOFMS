using Tofms.Common.JsonTofms.Models;
using Tofms.Common.Move;

namespace Tofms.Common;

public class TofmSystem
{
    public Dictionary<string, List<Location>> Journeys { get; set; }
    public IEnumerable<MoveAction> MoveActions { get; set; }
    public IEnumerable<string> Parts { get; set; }
    
}