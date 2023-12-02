﻿// See https://aka.ms/new-console-template for more information

using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;
using TACPN.Net;
using TapaalParser.TapaalGui;
using TapaalParser.TapaalGui.XmlWriters;
using Tofms.Common.JsonTofms;
using Tofms.Common.JsonTofms.ConsistencyCheck.Validators;

string ExtractTapaalXml(IEnumerable<PetriNetComponent> petriNetComponents)
{
    var sharedComponentStrings = petriNetComponents.Select(async component =>
        {
            GuiPositioningFinder positioningFinder = new GuiPositioningFinder(component);
            var positionalComponent = positioningFinder.GetComponentPlacements();

            TacpnComponentXmlParser parser = new TacpnComponentXmlParser(positionalComponent);
            return await parser.CreateXmlComponent();
        })
        .Select(e => e.Result);


    var builder = new StringBuilder();
    builder.Append($"<pnml xmlns=\"http://www.informatik.hu-berlin.de/top/pnml/ptNetb\">");
    var colourTypes = petriNetComponents.SelectMany(e => e.Places.Select(p => p.ColourType)).ToHashSet();
    var headerString = new ColourDeclarationWriter().XmlString(colourTypes);

    foreach (var componentStrings in sharedComponentStrings)
        builder.Append(petriNetComponents);

    builder.Append(@$"<feature isColored=""true"" isGame=""false"" isTimed=""true""/>{'\n'}</pnml>");
    return builder.ToString();
}

static bool IsValidFilePath(string filePath)
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
        if (IsValidFilePath(p))
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

JsonTofmToDomainTofmParser parser = new JsonTofmToDomainTofmParser(TofmSystemValidator.Default());
var system = await parser.ParseTofmsSystemJsonString(text);

TofmToTacpnTranslater translater = new TofmToTacpnTranslater(new MoveActionToTransitionFactory());
var components = system
    .Select(async e=>await translater.TranslateAsync(e))
    .Select(e=>e.Result);

var xml = ExtractTapaalXml(components);
await File.WriteAllTextAsync(outputPath, xml);
Console.WriteLine("Output has been written to " + outputPath);