using Common.JsonTofms.Fixtures;
using Common.JsonTofms.Models;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class JsonTofmParserTest: IClassFixture<CentrifugeFixture>
{
    private readonly string centrifugeText;

    public JsonTofmParserTest(CentrifugeFixture centrifuge)
    {
        this.centrifugeText = centrifuge.Text;
    }
    
    
    [Fact]
    public void CanParseCentrifuge()
    {
        var res = JsonConvert.DeserializeObject<TofmComponent>(this.centrifugeText);
        if (res is null) throw new Exception();
    }
    
    
    
    
    
}