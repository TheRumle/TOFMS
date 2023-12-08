namespace Xml;



public class Eq
{
    private int longestJourneyLength;
    public string VarId { get; set; }
    public int Number { get; set; }
    public override string ToString()
    {
        return $"{Number} eq {VarId}";
    }

    public Eq(int number, string varId, int journeyLength, int longestJourneyLength)
    {
        Number = number;
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
                          <finiteintrangeconstant value=""{this.Length}"">
                            <finiteintrange end=""{this.longestJourneyLength}"" start=""0""/>
                          </finiteintrangeconstant>
                        </subterm>
                      </equality>";
    }
}