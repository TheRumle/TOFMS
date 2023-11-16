namespace JsonFixtures.Fixtures;

public abstract class JsonTofmsFixture
{
    protected const string ValidComponentPath = "./Json/Systems/Valid/Components/";
    protected const string ValidSystemPath = "./Json/Systems/Valid/";
    protected const string InvalidComponentPath = "./Json/Systems/Invalid/Components/";
    protected const string InvalidSystemPath = "./Json/Systems/Invalid/";

    public static string ReadValidComponentWithName(string name)
    {
        string fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{ValidComponentPath}{fileName}");
    }
    
    
    public static string ReadInvalidComponentWithName(string name)
    {
        string fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{InvalidComponentPath}{fileName}");
    }
    
    public static string ReadValidSystemWithName(string name)
    {
        string fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{ValidSystemPath}{fileName}");
    }
    
    public static string ReadInvalidSystemWithName(string name)
    {
        string fileName = name.EndsWith("json") ? name : name + "json";
        return File.ReadAllText($"{InvalidSystemPath}{fileName}");
    }
}

public class CentrifugeFixture : JsonTofmsFixture
{
    public string ComponentText { get; private set; } = ReadValidComponentWithName("Centrifuge.json");
    public string SystemWithOnlyComponentText { get; private set; } = ReadValidSystemWithName("CentrifugeSystem.json");
}