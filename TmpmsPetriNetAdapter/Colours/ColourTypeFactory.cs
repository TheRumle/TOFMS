using TACPN.Colours.Type;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.Colours;

public class ColourTypeFactory
{
    public readonly IntegerRangedColour Journey;
    public static readonly SingletonColourType DotColour = ColourType.DefaultColorType;
    public readonly ColourType Parts;
    public readonly ProductColourType Tokens;
    public readonly ProductColourType Transitions;

    public const string JourneyName = "Journey"; 
    public const string TokenName = "Tokens"; 
    public const string PartsName = "Parts"; 
    public const string TransitionsColourName = "TokensDot"; 
    
    public ColourTypeFactory(IEnumerable<string> parts, JourneyCollection journeyCollection)
    {
        Parts =  new ColourType(PartsName, parts);
        Journey = new IntegerRangedColour(JourneyName, journeyCollection.JourneyLengths().MaxBy(e=>e.Value).Value);
        Tokens = new ProductColourType(TokenName, Parts, Journey);
        Transitions = new ProductColourType(TransitionsColourName, Tokens, DotColour);
    }
    
}