    namespace TACPN.Net;

public class CapacityPlace : IPlace<string>
{
    public TokenCollection Tokens { get; }
    public string Name { get; init; }
    public bool IsCapacityLocation { get; init; }
    public ColourType ColourType { get; }
    public bool IsProcessingPlace { get; init; }
    public IEnumerable<ColourInvariant<string>> ColourInvariants { get; init; }

    public CapacityPlace(string name, int capacity)
    {
        IsCapacityLocation = true;
        IsProcessingPlace = false;
        this.ColourInvariants = new List<ColourInvariant<string>>(){ColourInvariant.DotDefault};
        this.ColourType = ColourType.DefaultColorType;
        this.Name = name;
        this.Tokens = TokenCollection.DotColorTokenCollection(capacity);
    }

    public override string ToString()
    {
        return this.Name;
    }
}