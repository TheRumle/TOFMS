using System.Text.RegularExpressions;
using FluentAssertions;
using TACPN.Transitions;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters;
using Xunit.Sdk;

namespace TapaalParser.UnitTests;

public class TransitionXmlWriterTest : IClassFixture<TestOutputHelper>
{
    public TransitionXmlWriterTest(TestOutputHelper helper)
    {
        this._helper = helper;
        this._translater = new TransitionXmlWriter();
    }

    private const string TransitionXmlText = $@"<transition angle=""0"" displayName=""true"" id=""T0"" infiniteServer=""false"" name=""T0"" nameOffsetX=""0"" nameOffsetY=""0"" player=""0"" positionX=""465"" positionY=""405"" priority=""0"" urgent=""false""/>";
    private static readonly string WhiteSpaceFreeTransitionXmlText = Regex.Replace(TransitionXmlText, @"\s+", " ");
    private readonly TestOutputHelper _helper;
    private readonly TransitionXmlWriter _translater;

    private Placement<Transition> CreateTransitionWithoutArcs()
    {
        var position = new Position(465, 405);
        var transition = new Transition("T0");

        return new Placement<Transition>(transition, position);
    }

    [Fact]
    public void ParsesCorrectly_WithNoArcs()
    {
        string result = _translater.XmlString(CreateTransitionWithoutArcs());
        _helper.WriteLine(result);
        result.Should().Be(WhiteSpaceFreeTransitionXmlText);
    }
}