namespace JsonFixtures.Fixtures;

public class CentrifugeFixture : IDisposable
{
    public string Text { get; } = File.ReadAllText("./Json/Centrifuge.json");

    public void Dispose()
    {
    }
}