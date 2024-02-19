using Tmpms;
using TmpmsChecker.Domain;

namespace TmpmsChecker.Algorithm;

public interface ISearchHeuristic
{
    float CalculateCost(Configuration configuration);
}