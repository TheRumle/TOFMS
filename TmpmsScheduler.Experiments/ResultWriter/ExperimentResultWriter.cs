using System.Globalization;
using CsvHelper;
using TmpmsScheduler.Experiments.ExperimentDriver;

namespace TmpmsScheduler.Experiments.ResultWriter;

public sealed class ExperimentResultWriter : IDisposable, IAsyncDisposable
{
    private readonly string _fullPathToFile;
    private readonly StreamWriter _fileWriter;
    private readonly CsvWriter _csvWriter;

    internal ExperimentResultWriter(string fullPathToFile)
    {
        _fullPathToFile = fullPathToFile;
        _fileWriter = new StreamWriter(_fullPathToFile);
        _csvWriter = new CsvWriter(_fileWriter, CultureInfo.InvariantCulture);
    }

    public void Write(ExperimentResult request) => _csvWriter.WriteRecord(request);
    public void Write(IEnumerable<ExperimentResult> requests) => _csvWriter.WriteRecords(requests);
    public void WriteAsync(IEnumerable<ExperimentResult> requests) => _csvWriter.WriteRecordsAsync(requests);

    public void Dispose()
    {
        _fileWriter.Dispose();
        _csvWriter.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _fileWriter.DisposeAsync();
        await _csvWriter.DisposeAsync();
    }
}