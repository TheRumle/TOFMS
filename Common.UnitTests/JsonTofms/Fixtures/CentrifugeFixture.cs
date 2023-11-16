namespace Common.JsonTofms.Fixtures;

public class CentrifugeFixture : IDisposable
{
    public string Text { get; } = File.ReadAllText("JsonTofms/Fixtures/Json/Centrifuge.json");

    public void Dispose()
    {
    }
}