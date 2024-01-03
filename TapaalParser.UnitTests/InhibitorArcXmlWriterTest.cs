using System.Text.RegularExpressions;
using FluentAssertions;
using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.XmlWriters;
using Tmpms.Common;
using Xunit.Sdk;

namespace TapaalParser.UnitTests;

public class InhibitorArcXmlWriterTest : IClassFixture<TestOutputHelper>
{
    private readonly TestOutputHelper _helper;
    private readonly InhibitorArcXmlWriter _translater;

    public InhibitorArcXmlWriterTest(TestOutputHelper helper)
    {
        this._helper = helper;
        this._translater = new InhibitorArcXmlWriter();
    }
    
    
    private const string InhibitorArcTextXml =
        """
        <arc id="I0" inscription="[0,inf)" nameOffsetX="0" nameOffsetY="0" source="P4" target="T0" type="tapnInhibitor" weight="5">
              <hlinscription>
                <text>1'dot.all</text>
                <structure>
                  <numberof>
                    <subterm>
                      <numberconstant value="1">
                        <positive/>
                      </numberconstant>
                    </subterm>
                    <subterm>
                      <all>
                        <usersort declaration="dot"/>
                      </all>
                    </subterm>
                  </numberof>
                </structure>
              </hlinscription>
            </arc>
        """;
    
    private static readonly string WhiteSpaceFreeArcXmlText = Regex.Replace(InhibitorArcTextXml, @"\s+", " ");
    private static InhibitorArc CreateArc()
    {
      var dict = new Dictionary<string, int>();
      dict.Add(ColourType.DefaultColorType.ColourValues.First(), InfinityInteger.Positive);
      
      var transition = new Transition("T0");
      var place = new Place("P4", dict, ColourType.DefaultColorType);
      return transition.AddInhibitorFrom(place, 5);
    }
    
    [Fact]
    public void ParsesArcCorrectly()
    {
      var arc = CreateArc();
      string result = _translater.XmlString(arc);
      string res = Regex.Replace(result, @"\s+", " ");
      _helper.WriteLine(result);
      res.Should().Be(WhiteSpaceFreeArcXmlText);
    }
    
}