namespace Xml;

public class And
{
    public And(Eq lhs, Eq rhs)
    {
        Lhs = lhs;
        Rhs = rhs;
    }

    public Eq Lhs { get; set; }
    public Eq Rhs { get; set; }

    public override string ToString()
    {
        return $"({Lhs} and {Rhs})";
    }

    public string ToXmlString()
    {
        return $@"<and>
                    <subterm>
                      {Lhs.ToXmlTag()}
                    </subterm>
                    <subterm>
                      {Rhs.ToXmlTag()}
                    </subterm>
                  </and>";
    }
}