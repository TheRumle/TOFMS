using System.Runtime.CompilerServices;
using TACPN.Colours.Type;
using TACPN.Colours.Values;

namespace TmpmsPetriNetAdapter.Colours;

public class ColourVariableFactory
{
    private readonly ColourTypeFactory _colourTypeFactory;
    private readonly ColourType partColourType;
    private const string PREFIX = "Var";

    public ColourVariableFactory(ColourTypeFactory ctFactory)
    {
        this._colourTypeFactory = ctFactory;
        this.partColourType = ctFactory.Parts;
    }

    public static string VariableNameFor(string part)
    {
        return PREFIX + part;
    }
    
    public VariableDecrement DecrementForPart(string part)
    {
        return new VariableDecrement(new ColourVariable(VariableNameFor(part), _colourTypeFactory.Journey));
    }

    private static ColourVariable CreateIfValid(string name, ColourType colourType)
    {
        if (colourType.Colours.Any(e=>e.Value == name)) 
            throw new ArgumentException($"Cannot create variable with same name as assigned colour type: {string.Join(" ", colourType.Colours.Select(e=>e.Value))}");

        return new ColourVariable(name, colourType);
    }

    public ColourVariable VariableForPart(string part)
    {
        return new ColourVariable(VariableNameFor(part), _colourTypeFactory.Journey);
    }
}