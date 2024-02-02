using Tmpms.Common;
using Tmpms.Common.Json;
using Tmpms.Common.Json.Models;
using Tmpms.Common.Json.Validators;

namespace JsonFixtures.Tofms.Fixtures;

public abstract class TmpmsJsonReader  
{
    public const string ValidComponentPath = "./Tofms/Json/";
    
    protected TmpmsJsonReader(string fileName)
    {
        _system = new Lazy<Task<TimedMultipartSystem>>(() => Task.Run(()=>ReadComponent(fileName)));
    }
    
    private Lazy<Task<TimedMultipartSystem>> _system { get; init; }
    public TimedMultipartSystem System => _system.Value.Result;

    public static async Task<TimedMultipartSystem> ReadComponent(string file)
    {
        var text = File.ReadAllText(ValidComponentPath+file);
        var component = await new TmpmsJsonInputParser(new TimedMultiPartSystemJsonInputValidator())
            .ParseTofmsSystemJsonString(text);

        if (component is null || !component.Locations.Any() || !component.MoveActions.Any())
            throw new ArgumentException($"Could not deserialize {file} to component.");

        return component;
    }
    
    
}