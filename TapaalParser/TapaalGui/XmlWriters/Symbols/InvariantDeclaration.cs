using Tofms.Common;

namespace TapaalParser.TapaalGui.XmlWriters.Symbols;

public class InvariantDeclaration
{
    public InvariantDeclaration(Comparator comparator, string colorType, string color, int value)
    {
        if (value == InfinityInteger.Positive && comparator == Comparator.Lte)
            Comparator = Comparator.Lt;
        else 
            Comparator = comparator;
        
        ColorType = colorType;
        Color = color;
        NumericValue = value;
    }
    
    public static InvariantDeclaration LteInvariant(string colour, int value, string ct)
    {
        Comparator comparator = Comparator.Lte;
        if (value == InfinityInteger.Positive)
            comparator = Comparator.Lt;
        
        return new InvariantDeclaration(comparator, ct, colour, value);
    }

    
    public int NumericValue { get; set; }

    public Comparator Comparator { get; set; }
    public string ColorType { get; set; }
    public string Color { get; set; }

    public override string ToString()
    {
        return $@"<colorinvariant>
        <inscription inscription=""{this.Comparator.Value} {this.NumericValue}""/>
        <colortype name=""{this.ColorType}"">
          <color value=""{this.Color}""/>
        </colortype>
      </colorinvariant>";
    }
}