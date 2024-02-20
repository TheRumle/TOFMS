using System.Diagnostics;
using Common.Results;
using TmpmsChecker;
using TmpmsChecker.Algorithm;
using TmpmsScheduler.Experiments.ResultWriter;

namespace TmpmsScheduler.Experiments.ExperimentDriver;

public sealed class ExperimentRunner
{
    private readonly TimedManufacturingProblem _problem;
    private readonly IEnumerable<ISearchHeuristic> _heuristics;
    private readonly ExperimentResultWriter _writer;
    private static PerformanceCounter avgCounter64SampleBase;

    public ExperimentRunner(TimedManufacturingProblem problem, IEnumerable<ISearchHeuristic> heuristics, ResultWriterFactory factory)
    {
        _problem = problem;
        _heuristics = heuristics;
        _writer = factory.GetWriter(problem.ProblemName);
    }

    public void RunExperiments()
    {
        foreach (var searchHeuristic in _heuristics)
        {
            CollectGarbage();
            var before = Process.GetCurrentProcess().VirtualMemorySize64;
            SearchAlgorithm algorithm = SearchAlgorithm.WithDefaultConfigurationGenerator(_problem, searchHeuristic);
            Stopwatch stopwatch = Stopwatch.StartNew();
            var result = algorithm.Execute();
            stopwatch.Stop();
            var after = Process.GetCurrentProcess().VirtualMemorySize64;
            _writer.Write(ConstructCsvRow(stopwatch, result, algorithm, searchHeuristic, before - after));
        }
    }

    private void CollectGarbage()
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
            {IsSuccess: true} => ExperimentResult.Successful(_problem, result.Value, elapsed, algorithm.NumberOfConfigurationsExplored, searchHeuristic,kb), 
            {IsSuccess: false} => ExperimentResult.Failed(_problem, elapsed, algorithm.NumberOfConfigurationsExplored, searchHeuristic,kb)
        };
    }
}