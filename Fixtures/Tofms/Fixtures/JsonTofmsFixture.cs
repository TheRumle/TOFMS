using Newtonsoft.Json;
using Tmpms.Common;

namespace JsonFixtures.Tofms.Fixtures;

public abstract class JsonTofmsFixture
{
    protected const string ValidComponentPath = "./Tofms/Json/Systems/Valid/Components/";
    protected const string ValidSystemPath = "./Tofms/Json/Systems/Valid/";
    protected const string InvalidComponentPath = "./Tofms/Json/Systems/Invalid/Components/";
    protected const string InvalidSystemPath = ".Tofms/Json/Systems/Invalid/";

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
    
    protected static TimedMultipartSystem ReadComponent(string file)
    {
        var text = File.ReadAllText(file);
        var component = JsonConvert.DeserializeObject<TimedMultipartSystem>(text);

        if (component is null || !component.Locations.Any() || !component.MoveActions.Any())
            throw new ArgumentException($"Could not deserialize {file} to component.");

        return component;
    }
 
}