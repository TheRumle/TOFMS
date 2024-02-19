using Common.Results;
using Tmpms;
using TmpmsChecker.Domain;

namespace TmpmsChecker.Algorithm;

internal class SearchAlgorithm
{ 
    protected readonly CostBasedQueue<ReachedState> Open;
    protected readonly Dictionary<ReachedState, ReachedState> PreviousFor;
    protected readonly Dictionary<Configuration, float> CostSoFar = new();
    protected readonly ISearchHeuristic Heuristic;
    protected readonly IConfigurationGenerator _configurationGenerator;
    private readonly ReachedState _startAction;
    private TimedManufacturingProblem Problem { get; set; }

    public SearchAlgorithm(TimedManufacturingProblem problem, ISearchHeuristic heuristic, IConfigurationGenerator configurationGenerator)
    {
        _startAction = ReachedState.ZeroDelay(problem.StartConfiguration);
        Open = new CostBasedQueue<ReachedState>();
        Open.Enqueue(_startAction,1f);
        
        Problem = problem;
        Heuristic = heuristic;
        
        PreviousFor = new()
        {
            [_startAction] = _startAction
        };
        CostSoFar[Problem.StartConfiguration] = 0;
        _configurationGenerator = configurationGenerator;
    }


    public Result<Schedule> Execute()
    {
        var goal = Search();
        if (goal is null)
            return Result.Failure<Schedule>(Errors.CouldNotFindSolution($"{nameof(SearchAlgorithm)}"));
        return Result.Success(ConstructSchedule(goal));
    }

    private ReachedState? Search()
    {
        while (Open.Count > 0)
        {
            var current = Open.Dequeue();
            if (current.ReachedConfiguration.IsGoalConfigurationFor(Problem.GoalLocation))
                return current;
            
            var reachableStates = _configurationGenerator.GenerateConfigurations(current.ReachedConfiguration);
            EstimateCostsFor(reachableStates, current);
        }

        return null;
    }
    
    
    /// <summary>
    ///  Iterates over the reachable states and uses the <see cref="Heuristic"/> to calculate the estimated cost of exploring each state. 
    /// </summary>
    /// <param name="reachableStates"> The states to examine</param>
    /// <param name="previous"> The current configuration, used to get the actual cost.</param>
    private void EstimateCostsFor(IEnumerable<ReachedState> reachableStates, ReachedState previous)
    {
        foreach (var next in reachableStates)
        {
            var nextConfiguration = next.ReachedConfiguration;
            var nextHasBeenVisitedBefore = CostSoFar.TryGetValue(nextConfiguration, out var previousNextCost); 
            var costToReachNext = CostSoFar[previous.ReachedConfiguration] + next.ActionCost();
                
            //If next has not been visited before or cheaper path has been found, update the cost to reach next
            //If next has already been visited and this path is not cheaper do not explore by taking current path
            if (!nextHasBeenVisitedBefore || costToReachNext < previousNextCost)
            {
                CostSoFar[nextConfiguration] = costToReachNext;
                Open.Enqueue(next, costToReachNext + Heuristic.CalculateCost(nextConfiguration));
                PreviousFor[next] = previous;
            }
        }
    }

    private Schedule ConstructSchedule(ReachedState goal)
    {
        //Reconstruct path
        Stack<ReachedState> result = new Stack<ReachedState>();
        result.Push(goal);
        ReachedState prev = goal;
        do
        {
            prev = PreviousFor[prev];
            result.Push(prev);
        } while (!prev.Equals(_startAction));
        return new Schedule(result);
    }
}