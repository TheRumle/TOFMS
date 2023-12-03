using System.Text;
using TACPN.Net;
using TapaalParser.TapaalGui;
using TapaalParser.TapaalGui.XmlWriters;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

namespace ConsoleApp.ProgramBuilder;

public class ExtractTacpnXmlFormat
{
    private readonly IEnumerable<PetriNetComponent> _components;
    private readonly TranslateToTacpn _prevStep;

    public ExtractTacpnXmlFormat(IEnumerable<PetriNetComponent> components, TranslateToTacpn translateToTacpn)
    {
        this._components = components;
        this._prevStep = translateToTacpn;
    }

    public async Task<WriteToFile> TranslateToTapaalXml()
    {
        var xml = ExtractTapaalXml();
        return new WriteToFile(await xml, this._prevStep.PrevStep.OutputFile);
    }

    private async Task<string> ExtractTapaalXml()
    {
        var sharedComponentTasks = _components.Select(component =>
        {
            GuiPositioningFinder positioningFinder = new GuiPositioningFinder(component);
            var positionalComponent = positioningFinder.GetComponentPlacements();

            TacpnComponentXmlParser parser = new TacpnComponentXmlParser(positionalComponent);
            return parser.CreateXmlComponent();
        });

        var strings = await Task.WhenAll(sharedComponentTasks);
        var builder = new StringBuilder();
        builder.Append($"<pnml xmlns=\"http://www.informatik.hu-berlin.de/top/pnml/ptNetb\">");
        
        AppendToplevelDcls(builder);

        foreach (var componentStrings in strings)
            builder.Append(componentStrings);

        builder.Append(@$"<feature isColored=""true"" isGame=""false"" isTimed=""true""/>{'\n'}</pnml>");
        return builder.ToString();
    }

    private void AppendToplevelDcls(StringBuilder builder)
    {
        builder.Append($@" <declaration> <structure> <declarations>");
        var colourTypes = _components.SelectMany(e => e.Places.Select(p => p.ColourType)).DistinctBy(e=>e.Name).ToHashSet();
        var topDcls = new ColourDeclarationWriter().XmlString(colourTypes);
        builder.Append(topDcls);

        builder.Append($@" </declarations> </structure> </declaration> ");
    }
}