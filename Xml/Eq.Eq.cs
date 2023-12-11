namespace Xml;



public class Eq
{
    private int longestJourneyLength;
    public string VarId { get; set; }
    public int journeyVal { get; set; }
    public override string ToString()
    {
        return $"{journeyVal} eq {VarId}";
    }

    public Eq(int journeyVal, string varId, int journeyLength, int longestJourneyLength)
    {
        this.journeyVal = journeyVal;
        VarId = varId;
        this.Length = journeyLength;
        this.longestJourneyLength = longestJourneyLength;
    }

    public int Length { get; set; }

    public string ToXmlTag()
    {
        return $@"<equality>
                        <subterm>
                          <variable refvariable=""{VarId}""/>
                        </subterm>
                        <subterm>
                          <finiteintrangeconstant value=""{journeyVal}"">
                            <finiteintrange end=""{this.longestJourneyLength}"" start=""0""/>
                          </finiteintrangeconstant>
                        </subterm>
                      </equality>";
    }
}