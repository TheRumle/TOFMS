using System.Text;
using Common;
using TACPN;
using TACPN.Colours.Type;
using TACPN.Net;
using TACPN.Places;
using TapaalParser.TapaalGui;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;
using Tmpms.Common;

namespace ConsoleApp.ProgramBuilder;

public class ExtractTacpnXmlFormat
{
    private readonly IEnumerable<PetriNetComponent> _components;
    private readonly TranslateToTacpn _prevStep;
    private readonly JourneyCollection _journeyCollection;
    private readonly ValidatedTofmSystem _system;

    public ExtractTacpnXmlFormat(IEnumerable<PetriNetComponent> components, TranslateToTacpn translateToTacpn, JourneyCollection journeyCollection)
    {
        this._components = components;
        this._prevStep = translateToTacpn;
        _journeyCollection = journeyCollection;
        this._system = _prevStep.ValidatedTofmSystem;
    }

    public async Task<WriteToFile> TranslateToTapaalXml()
    {
        var xml = ExtractTapaalXml();
        return new WriteToFile(await xml, this._prevStep.PrevStep.OutputFile);
    }

    private async Task<string> ExtractTapaalXml()
    {
        var sharedComponentTasks = BeginTranslateAllComponents();
        
        var builder = new StringBuilder();
        builder.Append($"<pnml xmlns=\"http://www.informatik.hu-berlin.de/top/pnml/ptNetb\">");

        var t = _components.Select(e => e.Transitions.First()).First();
        
        var parts = new ColourType("Parts", _system.Parts);
        AppendToplevelDcls(builder,_components.SelectMany(e=>e.AllPlaces()),parts, _journeyCollection);
        foreach (var componentStrings in await Task.WhenAll(sharedComponentTasks))
            builder.Append(componentStrings);

        builder.Append(@$"<feature isColored=""true"" isGame=""false"" isTimed=""true""/>{'\n'}</pnml>");
        return builder.ToString();
    }

    private IEnumerable<Task<string>> BeginTranslateAllComponents()
    {
        var sharedComponentTasks = _components.Select(component =>
        {
            GuiPositioningFinder positioningFinder = new GuiPositioningFinder(component);
            var positionalComponent = positioningFinder.GetComponentPlacements();

            TacpnComponentXmlParser parser = new TacpnComponentXmlParser(positionalComponent);
            return parser.CreateXmlComponent();
        });
        return sharedComponentTasks;
    }

    private void AppendToplevelDcls(StringBuilder builder, IEnumerable<IPlace> places, ColourType parts,
        JourneyCollection j)
    {

        string topDcls = new CyclicEnumerationDeclarationWriter().XmlString(new []{parts});
        builder.Append(topDcls);
        var intRangeWriter = new SpecialVariableWriter(builder);
        int longestCount = 0;
        foreach (var jour in j) 
            longestCount = longestCount > jour.Value.Count() ? longestCount : jour.Value.Count();


        intRangeWriter.Write(ColourType.JourneyColourName, longestCount);
        builder.Append($@" </declarations> </structure> </declaration> ");

        var enumerable = places as IPlace[] ?? places.ToArray();
        foreach (var pl in enumerable.OfType<CapacityPlace>())
            AppendSharedPlaceDcl(builder, pl);
        
        foreach (var pl in enumerable.OfType<Place>())
            AppendSharedPlaceDcl(builder, pl);
        
    }

    private void AppendSharedPlaceDcl(StringBuilder builder, CapacityPlace pl)
    {
        builder.Append($@" <shared-place initialMarking=""{pl.Tokens.Count}"" invariant=""&lt; inf"" name=""{pl.Name}"">");
        AppendType(builder, pl);
        AppendInitialMarking(builder, pl);
        
        builder.Append("</shared-place>");
    }
    
    private void AppendSharedPlaceDcl(StringBuilder builder, Place pl)
    {
        builder.Append($@" <shared-place initialMarking=""{0}"" invariant=""&lt; inf"" name=""{pl.Name}"">");
        CreateColorInvs(pl, builder);
        AppendType(builder, pl);
        builder.Append("</shared-place>");

    }
    
    private void AppendType(StringBuilder builder, Place pl)
    {
        builder.Append(
            $@"<type><text>{ColourType.TokensColourType.Name}</text><structure><usersort declaration=""{ColourType.TokensColourType.Name}""/></structure></type>");
    }
    
    private void AppendType(StringBuilder builder, IPlace pl)
    {
        var colorTypeStr = pl.ColourType.Name == ColourType.DefaultColorType.Name
            ? pl.ColourType.Name.ToLower()
            : pl.ColourType.Name;
        builder.Append(
            $@"<type><text>{colorTypeStr}</text><structure><usersort declaration=""{colorTypeStr}""/></structure></type>");
    }


    
    private void CreateColorInvs(Place pl, StringBuilder builder)
    {
        foreach (var invariant in pl.ColourInvariants)
        {
            var invValue = invariant.MaxAge != Infteger.PositiveInfinity ? invariant.MaxAge.ToString() : "inf";
            builder.Append($@"<colorinvariant> <inscription inscription=""&lt; {invValue}""/> <colortype name=""{invariant.ColourType.Name}""> <color value=""{invariant.SecondColour}""/> <color value=""{invariant.FirstColour}""/> </colortype> </colorinvariant>");
        }
    }
    
    private void AppendInitialMarking(StringBuilder builder, CapacityPlace place)
    {
        TokenCollection tokens = place.Tokens;
        
        
        builder.Append("<hlinitialMarking> <text>");
        builder.Append(place.Tokens.ToColourExpression());
        builder.Append("</text>");
        
        
        var structureBuilder = new StructureExpressionAppender(builder);
        structureBuilder.AppendStructureText(place);
        builder.Append("  </hlinitialMarking>");
    }


    
}