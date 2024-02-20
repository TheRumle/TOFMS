﻿using Common.Results;
using Tmpms;
using TmpmsChecker.Algorithm.ConfigurationGeneration;

namespace TmpmsChecker.Algorithm;

public class SearchAlgorithm
{
    public int NumberOfConfigurationsExplored => CostSoFar.Keys.Count;
    private readonly CostBasedQueue<ReachedState> Open;
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

    public static SearchAlgorithm WithDefaultConfigurationGenerator(TimedManufacturingProblem problem, ISearchHeuristic heuristic)
    {
        return new SearchAlgorithm(problem, heuristic,
            ConfigurationGenerator.WithDefaultImplementations(problem.Actions));
    }

    
    /// <summary>
    /// Runs the model checker until a either a solution is found or the timeout is reached.
    /// </summary>
    /// <param name="timeOut">The time before the checker will stop exploring more configurations, resulting in a Failed result.</param>
    /// <returns>A result containing the schedule if found, failure otherwise</returns>
    public Result<Schedule> Execute(TimeSpan timeOut)
    {
        var goal = Search(new CancellationTokenSource(timeOut).Token);
        return ExtractResult(goal);
    }

    /// <summary>
    /// Runs the model checker until a either a solution is found or the timeout is reached.
    /// </summary>
    /// <param name="timeOut">The time before the checker will stop exploring more configurations, resulting in a Failed result.</param>
    /// <returns>A result containing the schedule if found, failure otherwise</returns>
    public async Task<Result<Schedule>> ExecuteAsync(TimeSpan timeOut)
    {
        var goal =await Task.Run(()=>Execute(timeOut));
        return goal;
    }

    private ReachedState? Search(CancellationToken token)
    {
        while (Open.Count > 0)
        {
            if (token.IsCancellationRequested) return null;
            
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
    
    private Result<Schedule> ExtractResult(ReachedState? goal)
    {
        return goal is null 
            ? Result.Failure<Schedule>(Errors.CouldNotFindSolution($"Could not find a solution to {nameof(Problem.ProblemName)}")) 
            : Result.Success(ConstructSchedule(goal));
    }
}