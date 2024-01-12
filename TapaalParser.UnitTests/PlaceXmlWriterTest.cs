using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using TACPN;
using TACPN.Colours.Type;
using TACPN.Net;
using TACPN.Places;
using TapaalParser.TapaalGui;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters;
using Tmpms.Common;
using Xunit.Sdk;

namespace TapaalParser.UnitTests;

public class PlaceXmlWriterTest : IClassFixture<TestOutputHelper>
{
  public PlaceXmlWriterTest(TestOutputHelper helper)
  {
    this.help = helper;
  }

  private PlaceXmlWriter _translater = new PlaceXmlWriter();

  private const string XmlString = @"<place displayName=""true"" id=""P0"" initialMarking=""11"" invariant=""&lt; inf"" name=""P0"" nameOffsetX=""0"" nameOffsetY=""0"" positionX=""360"" positionY=""405"">
      <type>
        <text>dot</text>
        <structure>
          <usersort declaration=""dot""/>
        </structure>
      </type>
      <hlinitialMarking>
        <text>(11'dot)</text>
        <structure>
          <add>
            <subterm>
              <numberof>
                <subterm>
                  <numberconstant value=""11"">
                    <positive/>
                  </numberconstant>
                </subterm>
                <subterm>
                  <useroperator declaration=""dot""/>
                </subterm>
              </numberof>
            </subterm>
          </add>
        </structure>
      </hlinitialMarking>
    </place>";

  public static string WhiteSpaceFreeXml = Regex.Replace(XmlString, @"\s+", " ");
  private readonly TestOutputHelper help;


  private static Place CreatePlace()
  {
    var ageInv = new KeyValuePair<string, int>("dot", InfinityInteger.Positive);
    return new Place("P0", new[] { ageInv }, ColourType.DefaultColorType)
    {
      Tokens = TokenCollection.Singleton(ColourType.DefaultColorType,"dot",11)
    };
  }

  
    
    [Fact]
    public void CanParseTest()
    {
      var input = new Placement<Place>(CreatePlace(), new Position(360, 405));;
      string result = _translater.XmlString(input);
      help.WriteLine(result);
      using (new AssertionScope())
      {
        result.Should().Be(WhiteSpaceFreeXml);
      }
    }
}