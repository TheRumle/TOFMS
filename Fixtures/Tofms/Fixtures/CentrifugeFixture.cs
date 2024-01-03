using Newtonsoft.Json;
using Tmpms.Common.JsonTofms.Models;

namespace JsonFixtures.Tofms.Fixtures;

public class CentrifugeFixture : JsonTofmsFixture
{
    public string ComponentText { get; private set; } = ReadValidComponentWithName("Centrifuge.json");
    public string SystemWithOnlyComponentText { get; private set; } = ReadValidSystemWithName("CentrifugeSystem.json");

    public TofmComponent AsComponent()
    {
        return JsonConvert.DeserializeObject<TofmComponent>(ComponentText)!;
    }
}