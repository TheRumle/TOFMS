using System.Text;
using Tofms.Common;

namespace Xml;

public class AllPlacesButEndEmptyQueryWriter
{
    private readonly StringBuilder _builder;
    private readonly IEnumerable<Location> _locations;

    public AllPlacesButEndEmptyQueryWriter(StringBuilder builder, IEnumerable<Location> locations)
    {
        this._builder = builder;
        this._locations = locations.Where(e => e.Name != Location.EndLocationName);
    }

    public void WriteQuery(string traceOption)
    {
        _builder.Append(
            $@"<query active=""true"" algorithmOption=""CERTAIN_ZERO"" approximationDenominator=""2"" capacity=""4"" colorFixpoint=""false"" coloredReduction=""false"" discreteInclusion=""false"" enableOverApproximation=""false"" enableUnderApproximation=""false"" extrapolationOption=""null"" gcd=""true"" hashTableSize=""null"" inclusionPlaces=""*NONE*"" name=""AllEmpty"" overApproximation=""true"" pTrie=""true"" partitioning=""false"" reduction=""true"" reductionOption=""VerifyDTAPN"" searchOption=""HEURISTIC"" symmetricVars=""false"" symmetry=""true"" timeDarts=""false"" traceOption=""{traceOption}"" type=""Default"" useQueryReduction=""true"" useSiphonTrapAnalysis=""false"" useStubbornReduction=""true"" useTarOption=""false"" useTarjan=""false"">
    <formula>
      <exists-path>
        <finally>
          ");
        if (_locations.Count() > 1)
        {
            _builder.Append("<conjunction>");
        }
      

        foreach (var location in _locations)
        {
            _builder.Append($@" <integer-eq>
              <tokens-count>
                <place>{location.Name}</place>
              </tokens-count>
              <integer-constant>0</integer-constant>
            </integer-eq>");
        }
      
        if (_locations.Count() >  1)
        {
            _builder.Append("</conjunction>");
        }
      
        _builder.Append($@"
        </finally>
      </exists-path>
    </formula>
  </query>");
    }

    public void WriteFastestTrace()
    {
      this.WriteQuery("FASTEST");
    }

    public void WriteXmlQuery()
    {
        this.WriteQuery("NONE");
      
    }
    
    
}