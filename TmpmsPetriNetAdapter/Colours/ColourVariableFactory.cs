using System.Runtime.CompilerServices;
using TACPN.Colours.Type;
using TACPN.Colours.Values;

namespace TmpmsPetriNetAdapter.Colours;

public class ColourVariableFactory
{
    private Dictionary<string, ColourVariable> _variables; 
    private readonly ColourTypeFactory _colourTypeFactory;
    private const string PREFIX = "Var";
    public IEnumerable<ColourVariable> CreatedVariables =>_variables.Values;

    public ColourVariableFactory(ColourTypeFactory ctFactory)
    {
        this._colourTypeFactory = ctFactory;
        this._variables = new Dictionary<string, ColourVariable>();
        foreach (var partColour in ctFactory.Parts.Colours)
            VariableForPart(partColour);
    }

    public static string VariableNameFor(string part)
    {
        return PREFIX + part;
    }
    
    public VariableDecrement DecrementForPart(string part)
    {
        return new VariableDecrement(VariableForPart(part));
    }

    public ColourVariable VariableForPart(string part)
    {
        if (_variables.TryGetValue(part, out var existingVariable))
            return existingVariable;
        
        
        var newVariable = new ColourVariable(VariableNameFor(part), _colourTypeFactory.Journey);
        _variables.Add(part,newVariable);
        return newVariable;
    }
}