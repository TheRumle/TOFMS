using Tmpms;

namespace JsonFixtures.Tofms.Fixtures;

public class AllJsonFilesFixture
{
    public IReadOnlyCollection<TimedMultipartSystem> Systems { get; }
    public AllJsonFilesFixture()
    {
        var readFileTasks = Directory.GetFiles(TmpmsJsonReader.ValidComponentPath, "*.json")
            .Select(TmpmsJsonReader.ReadComponent);
        
        Systems = Task.WhenAll(readFileTasks).Result;
    }
}