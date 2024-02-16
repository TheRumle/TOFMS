using Common.Results;
using Tmpms;

namespace TmpmsChecker.Algorithm;

internal abstract class SearchAlgorithm
{ 
    public abstract string AlgorithmName { get; }
    protected readonly CostBasedQueue<Configuration> Open;
    protected readonly Dictionary<Configuration, Configuration> CameFrom;
    protected readonly Dictionary<Configuration, float> CostSoFar = new();
    protected readonly ISearchHeuristic Heuristic;
    private readonly IActionExecutor _configurationGenerator;
    public TimedManufacturingProblem Problem { get; set; }

    public SearchAlgorithm(TimedManufacturingProblem problem, ISearchHeuristic heuristic, IActionExecutor actionExecutor)
    {
        Open = new CostBasedQueue<Configuration>();
        Open.Enqueue(problem.StartConfiguration,1f);
        Problem = problem;
        Heuristic = heuristic;
        CameFrom = new Dictionary<Configuration, Configuration>();
        CameFrom[problem.StartConfiguration] = problem.StartConfiguration;
        CostSoFar[Problem.StartConfiguration] = 0;
        _configurationGenerator = actionExecutor;
    }


    public Result<Schedule> Execute()
    {
        var goal = Search();
        if (goal is null)
            return Result.Failure<Schedule>(Errors.CouldNotFindSolution(AlgorithmName));
        return Result.Success(ConstructSchedule(goal));
    }

    private Configuration? Search()
    {
        while (Open.Count > 0)
        {
            var current = Open.Dequeue();
            if (current.IsGoalConfigurationFor(Problem.GoalLocation))
                break;
            
            var reachableStates = _configurationGenerator.ExecuteAction(current);
            EstimateCostsFor(reachableStates, current);
        }

        return null;
    }

    private Schedule ConstructSchedule(Configuration goal)
    {
        var result = new List<Configuration>();
        Configuration prev;

        prev = goal;
        result.Add(prev);
        do
        {
            prev = CameFrom[prev];
            result.Add(prev);
        } while (!prev.Equals(Problem.StartConfiguration));
        result.Reverse();

        
        
        
        
        //TODO reconstruct the schedule
        throw new NotImplementedException();
    }



    /// <summary>
    ///  Iterates over the reachable states and uses the <see cref="Heuristic"/> to calculate the estimated cost of exploring each state. 
    /// </summary>
    /// <param name="reachableStates"> The states to examine</param>
    /// <param name="current"> The current configuration, used to get the actual cost.</param>
    private void EstimateCostsFor(IEnumerable<ReachedState> reachableStates, Configuration current)
    {
        foreach (var next in reachableStates)
        {
            var nextConfiguration = next.ReachedConfiguration;
            var nextHasBeenVisitedBefore = CostSoFar.TryGetValue(nextConfiguration, out var previousNextCost); 
            var costToReachNext = CostSoFar[current] + next.ActionCost();
                
            //If next has not been visited before or cheaper path has been found, update the cost to reach next
            //If next has already been visited and this path is not cheaper do not explore by taking current path
            if (!nextHasBeenVisitedBefore || costToReachNext < previousNextCost)
            {
                var estimatedCost = costToReachNext + Heuristic.CalculateCost(nextConfiguration);
                CostSoFar[nextConfiguration] = costToReachNext;
                Open.Enqueue(nextConfiguration, estimatedCost);
                CameFrom[nextConfiguration] = current;
            }
        }
    }
}