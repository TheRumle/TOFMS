using System.Text.RegularExpressions;
using FluentAssertions;
using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.XmlWriters;
using Xunit.Sdk;

namespace TapaalParser.UnitTests;

public class IngoingArcXmlWriterTest: IClassFixture<TestOutputHelper>
{
  private readonly IngoingArcXmlWriter _translater;
  private readonly TestOutputHelper _helper;
  
  
  private static readonly string XmlString =
    $"""
     <arc id="Ai0" inscription="[0,inf)" nameOffsetX="0" nameOffsetY="0" source="P3" target="T0" type="timed" weight="1">
            <colorinterval>
             <inscription inscription="[5,12]"/>
             <colortype name="AB">
               <color value="a"/>
             </colortype>
           </colorinterval>
           <colorinterval>
             <inscription inscription="[0,8]"/>
             <colortype name="AB">
               <color value="b"/>
             </colortype>
           </colorinterval>
           <hlinscription>
             <text>(1'a + 3'b)</text>
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
                       <numberconstant value="3">
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
  
  private static readonly string WhiteSpaceFreeArcXmlText = Regex.Replace(XmlString, @"\s+", " ");
  
  public IngoingArcXmlWriterTest(TestOutputHelper helper)
  {
    this._helper = helper;
    this._translater = new IngoingArcXmlWriter();
  }

  private static IngoingArc CreateArc()
  {
    var transition = new Transition("T0");
    var place = new Place("P3", new List<KeyValuePair<string, int>>()
    {
      new ("a", 10),
      new ("b", 13),
    }, new ColourType("AB", new []{"a", "b"}));
    var guards = new List<ColourTimeGuard>()
    {
      new(3,"b", new Interval(0,8)),
      new(1,"a", new Interval(5,12))
     };
    
    
    return transition.AddInGoingFrom(place, guards);
  }
  
  [Fact]
  public void ParsesArcCorrectly()
  {
    var arc = CreateArc();
    string result = _translater.XmlString(arc);
    var res = Regex.Replace(result, @"\s+", " ");
    _helper.WriteLine(result);
    res.Should().Be(WhiteSpaceFreeArcXmlText);
  }
}