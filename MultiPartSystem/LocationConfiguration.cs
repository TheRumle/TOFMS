using System.Text;

namespace Tmpms;

public class LocationConfiguration
{
    private readonly Dictionary<string, List<Part>> _parts = new(new List<KeyValuePair<string, List<Part>>>());

    public LocationConfiguration(IEnumerable<string> partTypes)
    {
        foreach (var part in partTypes) _parts[part] = [];
    }

    private LocationConfiguration(IEnumerable<KeyValuePair<string, List<Part>>> parts, int size)
    {
        _parts = parts.ToDictionary();
        Size = size;
    }

    public int Size { get; private set; }

    public IReadOnlyDictionary<string, List<Part>> PartsByType => _parts.AsReadOnly();

    public IEnumerable<Part> AllParts => _parts.Values.SelectMany(e => e);

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

    public void Remove(Part part)
    {
        Size -= 1;
        _parts[part.PartType].Remove(part);
    }

    public void Remove(Part[] parts)
    {
        foreach (var partsPerType in parts.GroupBy(e => e.PartType))
            _parts[partsPerType.Key].AddRange(partsPerType);

        Size -= parts.Length;
    }

    public void Remove(IEnumerable<Part> parts)
    {
        var enumerable = parts as Part[] ?? parts.ToArray();
        foreach (var partsPerType in enumerable.GroupBy(e => e.PartType))
            _parts[partsPerType.Key].AddRange(partsPerType);

        Size -= enumerable.Length;
    }


    public override string ToString()
    {
        var b = new StringBuilder();
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
                oldKvp.Value.Select(part => new Part(part.PartType, part.Age, part.Journey)).ToList())), Size);
    }
}