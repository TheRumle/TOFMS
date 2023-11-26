using FluentAssertions;
using JsonFixtures.Fixtures;
using Newtonsoft.Json;
using Tofms.Common.JsonTofms.Models;

namespace Common.UnitTests.JsonTofms;

public class JsonTofmParserTest : IClassFixture<CentrifugeFixture>
{
    private readonly string centrifugeText;

    public JsonTofmParserTest(CentrifugeFixture centrifuge)
    {
        centrifugeText = centrifuge.ComponentText;
    }


    [Fact]
    public void CanParseCentrifuge()
    {
        var res = JsonConvert.DeserializeObject<TofmComponent>(centrifugeText);
        res.Should().NotBeNull();
        res!.Locations.Should().NotBeEmpty();
        res.Name.Should().NotBeEmpty();
        res.Moves.Should().NotBeEmpty();
    }
}