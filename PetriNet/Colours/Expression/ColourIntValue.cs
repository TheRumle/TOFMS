namespace TACPN.Colours.Expression;

public struct ColourIntValue : IColourValue
{
    public ColourIntValue(int value)
    {
        this.EnumerationValue = value;
        this.Value = EnumerationValue.ToString();
    }
    public int EnumerationValue { get; set; }


    public static implicit operator int(ColourIntValue value)
    {
        return value.EnumerationValue;
    }

    public string Value { get; }
}
