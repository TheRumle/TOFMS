using System.Text.RegularExpressions;
using FluentAssertions;
using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Colours;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters;
using Xunit.Sdk;

namespace TapaalParser.UnitTests;

public class OutgoingArcXmlWriterTest : IClassFixture<TestOutputHelper>
{
    private readonly OutgoingArcXmlWriter _translater;
    private readonly TestOutputHelper _helper;

    private const string ArcXmlString =
      $"""
       <arc id="Ao0" inscription="1" nameOffsetX="0" nameOffsetY="0" source="T0" target="P1" type="normal" weight="1">
             <hlinscription>
               <text>(1'a + 2'b)</text>
               <structure>
                 <add>
                   <subterm>
                     <numberof>
                       <subterm>
                         <numberconstant value="1">
                           <positive/>
                         </numberconstant>
                       </subterm>
                       <subterm>
                         <useroperator declaration="a"/>
                       </subterm>
                     </numberof>
                   </subterm>
                   <subterm>
                     <numberof>
                       <subterm>
                         <numberconstant value="2">
                           <positive/>
                         </numberconstant>
                       </subterm>
                       <subterm>
                         <useroperator declaration="b"/>
                       </subterm>
                     </numberof>
                   </subterm>
                 </add>
               </structure>
             </hlinscription>
           </arc>
       """;
    
    private static readonly string WhiteSpaceFreeArcXmlText = Regex.Replace(ArcXmlString, @"\s+", " ");
    
    public OutgoingArcXmlWriterTest(TestOutputHelper helper)
    {
        var arc = CreateArc();
        this._helper = helper;
        this._translater = new OutgoingArcXmlWriter();
    }

    private static OutGoingArc CreateArc()
    {
      var transition = new Transition("T0");
      
      var place = new Place("P1", new List<KeyValuePair<string, int>>()
      {
        new ("a", 10),
        new ("b", 13),
      }, new ColourType("AB",new []{"A", "B"}));
      
      var prods = new List<Production>()
      {
        new("a", 1), new("b", 2)
      };
      return transition.AddOutGoingTo(place, prods);
    }
    
    [Fact]
    public void ParsesArcCorrectly()
    {
      var arc = CreateArc();
      string result = _translater.XmlString(arc);
      _helper.WriteLine(result);
      result.Should().Be(WhiteSpaceFreeArcXmlText);
    }
}