using System.Text;
using FluentAssertions;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TapaalParser.TapaalGui.Writers;
using Tmpms.Common.Journey;
using Xml;

namespace TaapalParser.UnitTests.TapaalGui.DeclarationWriters;

public class ColourDeclarationWriterTest(MoveActionFixture fixture) : WriterTest(fixture)
{
    private string NewString(ColourDeclarationWriter colourDeclarationWriter)
    {
        colourDeclarationWriter.AppendAllText();
        return RemoveWhiteSpace(colourDeclarationWriter.ToString());
    }
    private string Write(Action<StringBuilder> write)
    {
        var builder = GetInitializedBuilder();
        write.Invoke(builder);
        return FinishWriting(builder);
    }
    private string GetJourneyColourValueOriginal()
    {
        return Write(builder =>
        {
            var declarer = new ColourDeclarer(builder);
            var jours = Journeys.ToIndexedJourney();
            declarer.WriteJourney(builder, jours);
        });
    }

    private StringBuilder GetInitializedBuilder()
    {
        var builder = new StringBuilder();
        builder.Append($"  <declaration>\n    <structure>\n      <declarations>");
        return builder;
    }

    private string FinishWriting(StringBuilder builder)
    {
        builder.Append($"      </declarations>\n    </structure>\n  </declaration>");
        return RemoveWhiteSpace(builder.ToString());
    }

    private ColourDeclarationWriter CreateSut(IEnumerable<ColourType> types, ColourVariable[] variables)
    {
        return new ColourDeclarationWriter((types,variables));
    }
    private ColourDeclarationWriter CreateSut(IEnumerable<ColourType> types)
    {
        return new ColourDeclarationWriter((types,[]));
    }

    private string OriginalTokenValue()
    {
        return Write(builder =>
        {
            var declarer = new ColourDeclarer(builder);
            var jours = Journeys.ToIndexedJourney();
            declarer.WriteTokenDeclaration(jours);
        });
    }

    public string OriginalDotComparison()
    {
        return Write(builder =>
        {
            var declarer = new ColourDeclarer(builder);
            declarer.WriteDot();
        });
    }
    
    public string GetPartsComparison(IEnumerable<string> parts)
    {
        return Write(builder =>
        {
            var declarer = new ColourDeclarer(builder);
            declarer.WriteParts(parts);
        });
    }


    [Fact]
    public void WhenGivenDotColourType_GivesCorrectValue()
    {
        ColourDeclarationWriter sut = CreateSut([Dot]);
        NewString(sut).Should().Be(OriginalDotComparison());
    }
    
    [Fact]
    public void Can_WriteJourney()
    {
        ColourDeclarationWriter sut = CreateSut([JourneyColour]);
        NewString(sut).Should().Be(GetJourneyColourValueOriginal());
    }

    [Fact]
    public void CanWriteTokenColour()
    {
        ColourDeclarationWriter sut = CreateSut([TokensColourType]);
        NewString(sut).Should().Be(OriginalTokenValue());
    }



    [Fact]
    public void WhenGivenDotAndParrtColourType_GivesCorrectValue()
    {
        ColourDeclarationWriter sut = CreateSut(new []{PartsColourType, Dot});
        var compare = Write(builder =>
        {
            var declarer = new ColourDeclarer(builder);
            declarer.WriteParts(Parts);
            declarer.WriteDot();
        });
        var newVal = NewString(sut);
        newVal.Should().Be(compare);
    }
    
    [Fact]
    public void WhenGivenPartColourType_GivesCorrectValue()
    {
        ColourDeclarationWriter sut = CreateSut([PartsColourType]);
        NewString(sut).Should().Be(GetPartsComparison(Parts));
    }
    
    
}