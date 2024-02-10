using System.Runtime.CompilerServices;
using System.Text;
using FluentAssertions;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TapaalParser.TapaalGui.Writers;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter;
using TmpmsPetriNetAdapter.Colours;
using Xml;

namespace TaapalParser.UnitTests;

public class ColourTypeDeclarationWriterTest : NoWhitespaceWriterTest, IClassFixture<MoveActionFixture>
{
    private readonly JourneyCollection _journeys;

    [Fact]
    public void WhenGivenDotColourType_GivesCorrectValue()
    {
        ColourTypeDeclarationWriter sut = CreateSut([Dot]);
        GetSutString(sut).Should().Be(GetDotComparison());
    }
    
    [Fact]
    public void Can_WriteJourney()
    {
        ColourTypeDeclarationWriter sut = CreateSut([JourneyColour]);
        GetSutString(sut).Should().Be(GetJourneyColourValueOriginal());
    }

    [Fact]
    public void CanWriteTokenColour()
    {
        ColourTypeDeclarationWriter sut = CreateSut([TokensColourType]);
        GetSutString(sut).Should().Be(GetOriginalTokenColourXml());
    }

    private string GetOriginalTokenColourXml()
    {
        var builder = new StringBuilder();
        var declarer = new ColourDeclarer(builder);
        var jours = _journeys.ToIndexedJourney();
        declarer.WriteTokenDeclaration(jours);
        return RemoveWhiteSpace(builder.ToString());
    }

    [Fact]
    public void WhenGivenDotAndParrtColourType_GivesCorrectValue()
    {
        ColourTypeDeclarationWriter sut = CreateSut(new []{PartsColourType, Dot});
        var compare = GetPartsComparison(Parts) + GetDotComparison();
        var newVal = GetSutString(sut);
        newVal.Should().Be(compare);
    }
    
    [Fact]
    public void WhenGivenPartColourType_GivesCorrectValue()
    {
        ColourTypeDeclarationWriter sut = CreateSut([PartsColourType]);
        GetSutString(sut).Should().Be(GetPartsComparison(Parts));
    }

    private string GetJourneyColourValueOriginal()
    {
        var builder = new StringBuilder();
        var declarer = new ColourDeclarer(builder);
        var jours = _journeys.ToIndexedJourney();
        declarer.WriteJourney(builder, jours);
        return RemoveWhiteSpace(builder.ToString());
    }

    private string GetSutString(ColourTypeDeclarationWriter colourTypeDeclarationWriter)
    {
        colourTypeDeclarationWriter.AppendToStringBuilder();
        return RemoveWhiteSpace(colourTypeDeclarationWriter.ToString());
    }

    private ColourTypeDeclarationWriter CreateSut(IEnumerable<ColourType> types)
    {
        return new ColourTypeDeclarationWriter(types);
    }



    public string GetDotComparison()
    {
        var builder = new StringBuilder();
        var declarer = new ColourDeclarer(builder);
        declarer.WriteDot();
        return RemoveWhiteSpace(builder.ToString());
    }
    
    public string GetPartsComparison(IEnumerable<string> parts)
    {
        var builder = new StringBuilder();
        var declarer = new ColourDeclarer(builder);
        declarer.WriteParts(parts);
        return RemoveWhiteSpace(builder.ToString());
    }



    public ColourTypeDeclarationWriterTest(MoveActionFixture fixture)
    {
        Parts = ["P1", "P2", "P3"];
        PartsColourType = new ColourType(Colour.PartsColour.Value, Parts);
        Dot = ColourType.DefaultColorType;
        _journeys = JourneyCollection.ConstructJourneysFor(
        [
            ("P1", [..fixture.LocationGenerator.GenerateLocations(3), ..fixture.LocationGenerator.GenerateLocations(1)]),
            ("P2", [..fixture.LocationGenerator.GenerateLocations(1), ..fixture.LocationGenerator.GenerateLocations(3)])
        ]);
        ColourFactory = new ColourTypeFactory(Parts, _journeys);
        JourneyColour = ColourFactory.Journey;
        TokensColourType = ColourFactory.Tokens;
    }

    public ColourTypeFactory ColourFactory { get; set; }

    public ProductColourType TokensColourType { get; set; }

    public ColourType JourneyColour { get; set; }

    public SingletonColourType Dot { get; set; }

    public ColourType PartsColourType { get; set; }

    public string[] Parts { get; set; }
}