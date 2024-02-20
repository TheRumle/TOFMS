using TmpmsChecker;
using TmpmsChecker.Algorithm;

namespace TmpmsScheduler.Experiments.ExperimentDriver;


[Serializable]
public record ExperimentResult(string ProblemName, string HeuristicName, int Makespan, double SecondsToExecute, int NumberConfigurations, double KbUsed, bool FoundSolution)
{

    public static ExperimentResult Successful(TimedManufacturingProblem problem, Schedule schedule, TimeSpan time, int numExploredConfigurations, ISearchHeuristic heuristic, double memoryUsed)
    {
        return new ExperimentResult(problem.ProblemName, heuristic.GetType().Name, schedule.TotalMakespan, time.TotalSeconds,numExploredConfigurations,  memoryUsed, true);
    } 
    
    public static ExperimentResult Failed(TimedManufacturingProblem problem, TimeSpan time, int numExploredConfigurations, ISearchHeuristic heuristic, double memoryUsed)
    {
        return new ExperimentResult(problem.ProblemName, heuristic.GetType().Name, Int32.MaxValue, time.TotalSeconds,numExploredConfigurations, memoryUsed, false);
    } 
}