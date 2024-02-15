using Tmpms;

namespace TMPMSChecker.Algorithm;

public interface ISearchHeuristic
{
    float CalculateCost(Configuration configuration);
}