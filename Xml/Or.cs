namespace Xml;

public class Or
{
    public Or(And lhs, And rhs)
    {
        Lhs = lhs;
        Rhs = rhs;
    }

    public And Lhs { get; set; }
    public And Rhs { get; set; }

    public override string ToString()
    {
        return $"({Lhs} or {Rhs})";
    }

    public string ToXmlString()
    {
        return $@"<or>
                <subterm>
                  {Lhs.ToXmlString()}
                </subterm>
                <subterm>
                  {Rhs.ToXmlString()}
                </subterm>
              </or>";
    }
}