namespace TmpmsChecker.Algorithm.Heuristics;

public class MinRemainingWorkspanFirst : ISearchHeuristic
{
    public float CalculateCost(Configuration configuration)
    {
        var totalMakespan = configuration
            .LocationConfigurations
            .Values
            .SelectMany(e => e.AllParts)
            .Sum(part => part.Journey.Sum(journeyLocation => journeyLocation.InvariantsByType[part.PartType].Max));
        return totalMakespan; 
    }
}