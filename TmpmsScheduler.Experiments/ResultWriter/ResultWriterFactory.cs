using System.Globalization;

namespace TmpmsScheduler.Experiments.ResultWriter;

/// <summary>
/// Creates a directory at the path with name yyyy.mm.dd.hh.mm format
/// </summary>
/// <param name="path"></param>
public class ResultWriterFactory
{
    private readonly DirectoryInfo _directory;

    public ResultWriterFactory(string path)
    {
        if (!Directory.Exists(path)) throw new ArgumentException($"{path} is not a directory");
        DateTime a = DateTime.Now;
        DateTime b = new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0, a.Kind);
        _directory = Directory.CreateDirectory(b.ToString(CultureInfo.InvariantCulture));
    }

    public ExperimentResultWriter GetWriter(string fileName)
    {
        return new ExperimentResultWriter(Path.Combine(_directory.FullName, fileName));
    }
}