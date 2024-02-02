using Newtonsoft.Json;
using Tmpms.Common;
using Tmpms.Common.Json.Models;

namespace JsonFixtures.Tofms.Fixtures;

public class CentrifugeFixture : JsonTofmsFixture
{
    public string ComponentText { get; private set; } = ReadValidComponentWithName("Centrifuge.json");
    public string SystemWithOnlyComponentText { get; private set; } = ReadValidSystemWithName("CentrifugeSystem.json");

    public TimedMultipartSystem
        AsComponent()
    {
        return JsonConvert.DeserializeObject<TimedMultipartSystem>(ComponentText)!;
    }
}