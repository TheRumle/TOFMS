using System.Text;
using TACPN.Net;
using TapaalParser.TapaalGui;
using TapaalParser.TapaalGui.XmlWriters;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;
using Tofms.Common;

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
        
        AppendToplevelDcls(builder,this._components.SelectMany(e=>e.Places));
        foreach (var componentStrings in strings)
            builder.Append(componentStrings);

        builder.Append(@$"<feature isColored=""true"" isGame=""false"" isTimed=""true""/>{'\n'}</pnml>");
        return builder.ToString();
    }

    private void AppendToplevelDcls(StringBuilder builder, IEnumerable<Place> places)
    {
        builder.Append($@" <declaration> <structure> <declarations>");
        var colourTypes = _components.SelectMany(e => e.Places.Select(p => p.ColourType)).DistinctBy(e=>e.Name).ToHashSet();
        var topDcls = new ColourDeclarationWriter().XmlString(colourTypes);
        builder.Append(topDcls);
        builder.Append($@" </declarations> </structure> </declaration> ");


        foreach (var pl in places)
        {
            AppendSharedPlaceDcl(builder, pl);

        }
        
    }

    private void AppendSharedPlaceDcl(StringBuilder builder, Place pl)
    {
        builder.Append($@" <shared-place initialMarking=""{pl.Tokens.Count}"" invariant=""&lt; inf"" name=""{pl.Name}"">");
        CreateColorInvs(pl, builder);
        AppendType(builder, pl);
        if (pl.Tokens.Count > 0)
        {
            AppendInitialMarking(builder, pl.Tokens);
        }
        
        builder.Append("</shared-place>");

    }

    private void AppendType(StringBuilder builder, Place pl)
    {
        var colorTypeStr = pl.ColourType.Name == ColourType.DefaultColorType.Name
            ? pl.ColourType.Name.ToLower()
            : pl.ColourType.Name;
        builder.Append(
            $@"<type><text>{colorTypeStr}</text><structure><usersort declaration=""{colorTypeStr}""/></structure></type>");
    }

    private void CreateColorInvs(Place pl, StringBuilder builder)
    {
        foreach (var r in pl.ColorInvariants)
        {
            var colorTypeStr = pl.ColourType.Name == ColourType.DefaultColorType.Name
                ? pl.ColourType.Name.ToLower()
                : pl.ColourType.Name;
            
            var invValue = r.Value != Infteger.PositiveInfinity ? r.Value.ToString() : "inf";
            builder.Append($@"<colorinvariant> <inscription inscription=""&lt; {invValue}""/> <colortype name=""{colorTypeStr}""> <color value=""{r.Key}""/> </colortype> </colorinvariant>");
        }
    }
    
    private void AppendInitialMarking(StringBuilder builder, TokenCollection tokens)
    {
        builder.Append("<hlinitialMarking> <text>");

        var colourExpressionBuilder = new ColorExpressionAppender(builder);
        colourExpressionBuilder.WriteColourExpression(tokens);
        builder.Append("</text>");
        var structureBuilder = new StructureExpressionAppender(builder);
        structureBuilder.AppendStructureText(tokens);
        builder.Append("  </hlinitialMarking>");
    }
}