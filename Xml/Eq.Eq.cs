namespace Xml;



public class Eq
{
    public string Var { get; set; }
    public int Number { get; set; }
    public override string ToString()
    {
        return $"{Number} eq {Var}";
    }

    public Eq(int number, string var, int journeyLength)
    {
        Number = number;
        Var = var;
        this.Length = journeyLength;

    }

    public int Length { get; set; }

    public string ToXmlTag()
    {
        return $@"<equality>
                        <subterm>
                          <variable refvariable=""{Var}""/>
                        </subterm>
                        <subterm>
                          <finiteintrangeconstant value=""0"">
                            <finiteintrange end=""{this.Length}"" start=""0""/>
                          </finiteintrangeconstant>
                        </subterm>
                      </equality>";
    }
}