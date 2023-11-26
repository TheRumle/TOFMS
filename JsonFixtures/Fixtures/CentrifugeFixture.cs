using Newtonsoft.Json;
using Tofms.Common.JsonTofms.Models;

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