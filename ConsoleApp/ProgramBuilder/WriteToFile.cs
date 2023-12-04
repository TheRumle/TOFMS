namespace ConsoleApp.ProgramBuilder;

public class WriteToFile
{
    private readonly string _content;
    private string _path;

    public WriteToFile(string xml, string prevStep)
    {
        this._content = xml;
        this._path = prevStep;
    }

    public async Task WriteToOutputPath()
    {
        if (!_path.EndsWith(".xml"))
            _path += (".xml");
        
        await File.WriteAllTextAsync(_path, _content);
        Console.WriteLine($"SUCCESS! Written to {_path}");

    }
}