    namespace TACPN.Net;

public class CapacityPlace : IPlace<string>
{
    public TokenCollection Tokens { get; init; }
    public string Name { get; init; }
    public bool IsCapacityLocation { get; init; }
    public ColourType ColourType { get; }
    public bool IsProcessingPlace { get; init; }
    public IEnumerable<ColourInvariant<string>> Invariant { get; init; }

    public CapacityPlace(string name, ColourInvariant<string> invariant, int capacity)
    {
        if (invariant.ColourType != ColourType.DefaultColorType) throw new Exception("Cannot create cap. place from inv. with color type" + invariant.ColourType);
        IsCapacityLocation = true;
        IsProcessingPlace = false;
        this.Invariant = new List<ColourInvariant<string>>(){invariant};
        this.ColourType = invariant.ColourType;
        this.Name = name;

        var tokens = new List<Token>();
        for (int i = 0; i < capacity; i++)
        {
            tokens.Add(new Token(ColourType.DefaultColorType.Colours.First()));   
        }
        
        this.Tokens = new TokenCollection(invariant.ColourType, tokens)
        {
            
        };

    }
}