using System.Text;

namespace Tmpms;

public class LocationConfiguration
{
    public int Size { get; private set; }
    
    private Dictionary<string, List<Part>> _parts = new(new List<KeyValuePair<string, List<Part>>>());

    public IReadOnlyDictionary<string, List<Part>> PartsByType => _parts.AsReadOnly();
    
    public IEnumerable<Part> AllParts => _parts.Values.SelectMany(e=>e);
    
    public LocationConfiguration(IEnumerable<string> partTypes)
    {
        foreach (var part in partTypes) _parts[part] = [];
    }

    private LocationConfiguration( IEnumerable<KeyValuePair<string, List<Part>>> parts, int size)
    {
        _parts = parts.ToDictionary();
        Size = size;
    }
    
    public void Add(Part part)
    {
        Size += 1;
        _parts[part.PartType].Add(part);
    }
    
    public void Add(Part[] parts)
    {
        foreach (var partsPerType in parts.GroupBy(e => e.PartType))
            _parts[partsPerType.Key].AddRange(partsPerType);

        Size += parts.Length;
    }
    
    public void Add(IEnumerable<Part> parts)
    {
       Add(parts.ToArray());
    }
    
    
    
    public override string ToString()
    {
        StringBuilder b = new StringBuilder();
        foreach (var (color, parts) in _parts)
        {
            b.Append($"{color}: [");
            b.Append(string.Join(", ", parts));
            b.Append(']');
        }

        return b.ToString();
    }
    
    public LocationConfiguration Copy()
    {
        return new LocationConfiguration(_parts
            .Select(oldKvp => KeyValuePair.Create(oldKvp.Key,
            oldKvp.Value.Select(part=>new Part(part.PartType, part.Age, part.Journey)).ToList())), Size);
    }
}