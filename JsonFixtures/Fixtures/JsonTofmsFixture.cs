using System.Collections.Concurrent;
using Newtonsoft.Json;
using Tofms.Common.JsonTofms.Models;

namespace JsonFixtures.Fixtures;

public abstract class JsonTofmsFixture
{
    protected const string ValidComponentPath = "./Json/Systems/Valid/Components/";
    protected const string ValidSystemPath = "./Json/Systems/Valid/";
    protected const string InvalidComponentPath = "./Json/Systems/Invalid/Components/";
    protected const string InvalidSystemPath = "./Json/Systems/Invalid/";

    public static string ReadValidComponentWithName(string name)
    {
        var fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{ValidComponentPath}{fileName}");
    }


    public static string ReadInvalidComponentWithName(string name)
    {
        var fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{InvalidComponentPath}{fileName}");
    }

    public static string ReadValidSystemWithName(string name)
    {
        var fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{ValidSystemPath}{fileName}");
    }

    public static string ReadInvalidSystemWithName(string name)
    {
        var fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{InvalidSystemPath}{fileName}");
    }
    
    protected static void ReadComponent(string file, ConcurrentQueue<string> partTypes,
        ConcurrentQueue<TofmComponent> components)
    {
        var text = File.ReadAllText(file);
        var component = JsonConvert.DeserializeObject<TofmComponent>(text);

        if (component is null || !component.Locations.Any() || !component.Moves.Any())
            throw new ArgumentException($"Could not deserialize {file} to component.");

        AppendParts(component, partTypes);
        components.Enqueue(component);
    }
    
    protected static void AppendParts(TofmComponent component, ConcurrentQueue<string> parts)
    {
        component.Moves.ForEach(action => action.Parts.ForEach(p => parts.Enqueue(p.PartType)));
    }
}