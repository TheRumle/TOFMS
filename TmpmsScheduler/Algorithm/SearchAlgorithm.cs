using Common.Errors;
using Common.Results;
using Tmpms;

namespace TMPMSChecker.Algorithm;

public interface IActionExecutor
{
    public IEnumerable<Configuration> ExecuteActions(Configuration configuration);
}

public abstract class SearchAlgorithm
{ 
    public abstract string AlgorithmName { get; }
    protected readonly Location Goal;
    protected readonly CostBasedQueue<Configuration> Open;
    protected readonly List<ReachedState> CameFrom;
    protected readonly Dictionary<Configuration, float> CostSoFar = new();
    public readonly IActionExecutor ConfigurationGenerator;

    protected readonly ISearchHeuristic Heuristic;
    
    public SearchAlgorithm(TimedManufacturingProblem problem, ISearchHeuristic heuristic, IActionExecutor actionExecutor)
    {
        Open = new CostBasedQueue<Configuration>();
        Open.Enqueue(problem.StartConfiguration,1f);
        Goal = problem.GoalLocation;
        Heuristic = heuristic;
        CameFrom = [];
        this.ConfigurationGenerator = actionExecutor;
    }


    public Result<Schedule> Execute()
    {
        var goal = Search();
        if (goal is null)
            return Result.Failure<Schedule>(Errors.CouldNotFindSolution(AlgorithmName));
        return Result.Success(ConstructSchedule());
    }

    protected abstract Configuration? Search();

    private Schedule ConstructSchedule()
    {
           
        
        
        //TODO reconstruct the schedule
        throw new NotImplementedException();
    }

    protected void AddWithCost()
    {
        while (Open.Count > 0)
        {
            var current = Open.Dequeue();

            //Comment out if your want best possible path
            if (current.IsGoalConfigurationFor(Goal))
                break;

            //GENERATE
            var neighbors = ConfigurationGenerator.ExecuteActions(current);
            foreach (var next in neighbors)
            {
                var newCost = CostSoFar[current] + ActualCost(current,next);
                if (!CostSoFar.TryGetValue(next, out var storedNextCost) || newCost < storedNextCost)
                {
                    CostSoFar[next] = newCost;
                    var estimatedCost = newCost + Heuristic.CalculateCost(next);
                    Open.Enqueue(next, estimatedCost);
                    _cameFrom[next] = current;
                }
            }
        }
    }
}