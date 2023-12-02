using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;
using TACPN.Net;
using TapaalParser.TapaalGui;
using TapaalParser.TapaalGui.XmlWriters;
using Tofms.Common.JsonTofms;
using Tofms.Common.JsonTofms.ConsistencyCheck.Validators;
using Tofms.Common.Move;

namespace ConsoleApp;

public class TranslateToTacpn
{
    private readonly IEnumerable<MoveAction> moveActions;
    public readonly ParseAndValidateTofmSystem PrevStep;

    public TranslateToTacpn(IEnumerable<MoveAction> moveActions, ParseAndValidateTofmSystem parseAndValidateTofmSystem)
    {
        this.moveActions = moveActions;
        this.PrevStep = parseAndValidateTofmSystem;
    }

    public ExtractTacpnXmlFormat TranslateTofmsToTacpnComponent()
    {
        TofmToTacpnTranslater translater = new TofmToTacpnTranslater(new MoveActionToTransitionFactory());
        var components = moveActions
            .Select(async e=>await translater.TranslateAsync(e))
            .Select(e=>e.Result);

        return new ExtractTacpnXmlFormat(components, this);
    }
}

public class ExtractTacpnXmlFormat
{
    private readonly IEnumerable<PetriNetComponent> _components;
    private readonly TranslateToTacpn _prevStep;

    public ExtractTacpnXmlFormat(IEnumerable<PetriNetComponent> components, TranslateToTacpn translateToTacpn)
    {
        this._components = components;
        this._prevStep = translateToTacpn;
    }

    public WriteToFile TranslateToTapaalXml()
    {
        var xml = ExtractTapaalXml();
        return new WriteToFile(xml, this._prevStep.PrevStep.OutputFile);
    }

    private string ExtractTapaalXml()
    {
        var sharedComponentStrings = _components.Select(async component =>
            {
                GuiPositioningFinder positioningFinder = new GuiPositioningFinder(component);
                var positionalComponent = positioningFinder.GetComponentPlacements();

                TacpnComponentXmlParser parser = new TacpnComponentXmlParser(positionalComponent);
                return await parser.CreateXmlComponent();
            })
            .Select(e => e.Result);


        var builder = new StringBuilder();
        builder.Append($"<pnml xmlns=\"http://www.informatik.hu-berlin.de/top/pnml/ptNetb\">");
        var colourTypes = _components.SelectMany(e => e.Places.Select(p => p.ColourType)).ToHashSet();
        var headerString = new ColourDeclarationWriter().XmlString(colourTypes);

        foreach (var componentStrings in sharedComponentStrings)
            builder.Append(_components);

        builder.Append(@$"<feature isColored=""true"" isGame=""false"" isTimed=""true""/>{'\n'}</pnml>");
        return builder.ToString();
    }
}

public class WriteToFile
{
    private readonly string _content;
    private readonly string _path;

    public WriteToFile(string xml, string prevStep)
    {
        this._content = xml;
        this._path = prevStep;
    }

    public async Task WriteToOutputPath()
    {
        await File.WriteAllTextAsync(_path, _content);
    }
}

public class ParseAndValidateTofmSystem
{
    private readonly JsonTofmToDomainTofmParser _parser;
    private readonly string _system;
    public readonly string InputFile;
    public readonly string OutputFile;

    public ParseAndValidateTofmSystem(string inputFile, string outputFile, string tofmSystem)
    {
        this._system = tofmSystem;
        this.InputFile = inputFile;
        this.OutputFile = outputFile;
        this._parser = new JsonTofmToDomainTofmParser(TofmSystemValidator.Default());
    }

    public TranslateToTacpn ParseAndValidateInputSystem()
    {
        var moveActions =  _parser.ParseTofmsSystemJsonString(_system).Result;
        return new TranslateToTacpn(moveActions, this);
    }
}

public class TofmsToTapaal
{
    public static ParseAndValidateTofmSystem GetInOutFiles()
    {
        Console.WriteLine("Path to TOFMS system: ");
        string? inputPath = null;
        do
        {
            try
            {
                inputPath = Path.GetFullPath(Console.ReadLine());
            }
            catch (Exception _ignore)
            {
                Console.WriteLine("Not a valid path");
                Console.WriteLine("");
            }
        } while (String.IsNullOrWhiteSpace(inputPath) );
        string? outputPath= null;
        do
        {
            try
            {
                var p = Console.ReadLine()!;
                if (FileValidation.IsValidFilePath(p))
                    outputPath = p;
                else
                {
                    Console.WriteLine(p + " was not a valid path to write file");
                }
            }
            catch (Exception _ignore)
            {
                Console.WriteLine("Not a valid path");
            }

        } while (String.IsNullOrWhiteSpace(outputPath));
        
        string text = File.ReadAllText(inputPath);
        return new ParseAndValidateTofmSystem(inputPath, outputPath, text);
    }
}

public class FileValidation
{
    
    public static bool IsValidFilePath(string filePath)
    {
        try
        {
            // Check if the file path is rooted
            if (!Path.IsPathRooted(filePath))
            {
                return false;
            }

            // Check if the file name is valid
            string fileName = Path.GetFileName(filePath);
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return false;
            }

            // Check if the directory path is valid
            string directoryPath = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directoryPath) || directoryPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return false;
            }

            // Additional checks can be added as needed

            // If all checks pass, the file path is considered valid
            return true;
        }
        catch (Exception)
        {
            // An exception occurred, indicating the file path is not valid
            return false;
        }
    }



}