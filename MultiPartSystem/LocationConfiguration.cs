using System.Text;

namespace Tmpms.Common;

public class LocationConfiguration() : Dictionary<string, List<Part>>(new List<KeyValuePair<string, List<Part>>>())
{
    public int Size => Values.SelectMany(e => e).Count();
    public IEnumerable<Part> Tokens => Values.SelectMany(e=>e); 
    public override string ToString()
    {
        StringBuilder b = new StringBuilder();
        foreach (var (color, parts) in this)
        {
            b.Append($"{color}: [");
            b.Append(string.Join(", ", parts));
            b.Append(']');
        }

        return b.ToString();
    }

    public void AddToken(Part part)
    {
        if (TryGetValue(part.PartType, out var parts))
        {
            parts.Add(part);
            return;
        }

        this[part.PartType] = [part];
    }
    
    public void AddToken(string value, int age)
    {
        AddToken(new Part(value,age));
    }
    public void AddToken((string value, int age) part)
    {
        AddToken(new Part(part.value,part.age));
    }
}