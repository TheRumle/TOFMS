using TmpmsChecker;
using TmpmsChecker.Algorithm;

namespace TmpmsScheduler.Experiments.ExperimentDriver;


[Serializable]
public record ExperimentResult(string ProblemName, string Algorithm, string HeuristicName, int Makespan, double SecondsToExecute, int NumberConfigurations, double KbUsed, bool FoundSolution)
{

    public static ExperimentResult Failed(TimedManufacturingProblem problem, SearchAlgorithm algorithm, ISearchHeuristic heuristic,TimeSpan time, double memoryUsed)
    {
        return Create(problem, algorithm, Int32.MaxValue, time, heuristic, memoryUsed, false);
    }

    public static ExperimentResult Successful(TimedManufacturingProblem problem, SearchAlgorithm algorithm, ISearchHeuristic heuristic,
        TimeSpan time, double memoryUsed, Schedule schedule)
    {
        return Create(problem, algorithm, schedule.TotalMakespan, time, heuristic, memoryUsed, true);
    }

    private static ExperimentResult Create(TimedManufacturingProblem problem, SearchAlgorithm algorithm,
        int makeSpan, TimeSpan elapsed, ISearchHeuristic heuristic, double kbUsed, bool isSuccess)
    {
        return new ExperimentResult(problem.ProblemName, algorithm.GetType().FullName!, heuristic.GetType().FullName!,
            makeSpan, elapsed.TotalSeconds, algorithm.NumberOfConfigurationsExplored, kbUsed, isSuccess);
    }
}