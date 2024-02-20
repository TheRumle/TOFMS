namespace TmpmsChecker.Algorithm;

public interface ISearchHeuristic
{
    float CalculateCost(Configuration configuration);
}