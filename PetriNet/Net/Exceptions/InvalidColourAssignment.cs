using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Exceptions;

public class InvalidColourAssignment : ArgumentException
{
    public InvalidColourAssignment(ColourType first, IColourValue value)
    : base($"Colour {first.Name} is not compatible with {value.Value}")
    {
        
    }

    public InvalidColourAssignment(IEnumerable<IColourValue> values, ColourType colourType)
    :base($@"Cannot make assignment of colour values ""{string.Join(", ", values.Select(e=>e.Value))}"" with colour type {colourType.Name} because at least one of the colours does not belong to the colour type")
    {
        
    }
}