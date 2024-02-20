using System.Diagnostics;
using Common.Results;
using TmpmsChecker;
using TmpmsChecker.Algorithm;
using TmpmsScheduler.Experiments.ResultWriter;

namespace TmpmsScheduler.Experiments.ExperimentDriver;

public sealed class ExperimentRunner
{
    private readonly ExperimentResultWriter _resultWriter;
    private readonly TimedManufacturingProblem _problem;
    private readonly IEnumerable<ISearchHeuristic> _heuristics;
    private readonly TimeSpan _timeOut;

    public ExperimentRunner(TimedManufacturingProblem problem,
        IEnumerable<ISearchHeuristic> heuristics,
        ResultWriterFactory factory, TimeSpan timeOut)
    {
        _problem = problem;
        _heuristics = heuristics;
        _resultWriter = factory.GetWriter(problem.ProblemName);
        _timeOut = timeOut;
    }

    public List<ExperimentResult> RunExperiments()
    {
        Stopwatch stopwatch = new Stopwatch();
        List<ExperimentResult> results = new List<ExperimentResult>();
        foreach (var searchHeuristic in _heuristics)
        {
            CollectGarbage();
            results.Add(RunExperiment(searchHeuristic, stopwatch));
        }

        return results;
    }

    private ExperimentResult RunExperiment(ISearchHeuristic searchHeuristic, Stopwatch stopwatch)
    {
        SearchAlgorithm algorithm = SearchAlgorithm.WithDefaultConfigurationGenerator(_problem, searchHeuristic);
        
        var memoryBefore = Process.GetCurrentProcess().VirtualMemorySize64;
        stopwatch.Restart();
        var scheduleResult = algorithm.Execute(_timeOut);
        stopwatch.Stop();
        var memoryAfter = Process.GetCurrentProcess().VirtualMemorySize64;

        var experimentResult = ConstructCsvRow(stopwatch, scheduleResult, algorithm, searchHeuristic,
            memoryBefore - memoryAfter);
        _resultWriter.Write(experimentResult);
        return experimentResult;
    }

    private static void CollectGarbage()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    private ExperimentResult ConstructCsvRow(Stopwatch stopwatch, Result<Schedule> result, SearchAlgorithm algorithm,
        ISearchHeuristic searchHeuristic, long virtualMemoryUsedBt)
    {
        var elapsed = stopwatch.Elapsed;
        var kb = virtualMemoryUsedBt / 1000;
        
        return result switch
        {
            {IsSuccess: true} => ExperimentResult.Successful(_problem, result.Value, elapsed,
                algorithm.NumberOfConfigurationsExplored, searchHeuristic,kb), 
            {IsSuccess: false} => ExperimentResult.Failed(_problem, elapsed,
                algorithm.NumberOfConfigurationsExplored, searchHeuristic,kb)
        };
    }
}