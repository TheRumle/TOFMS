using Common.JsonTofms.Models;
using Newtonsoft.Json;

namespace JsonFixtures.Fixtures;

public class CentrifugeFixture : JsonTofmsFixture
{
    public string ComponentText { get; private set; } = ReadValidComponentWithName("Centrifuge.json");
    public string SystemWithOnlyComponentText { get; private set; } = ReadValidSystemWithName("CentrifugeSystem.json");

    public TofmComponent AsComponent()
    {
        return JsonConvert.DeserializeObject<TofmComponent>(ComponentText)!;
    }
}