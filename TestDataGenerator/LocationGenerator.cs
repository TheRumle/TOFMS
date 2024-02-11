using Bogus;
using Tmpms.Common;

namespace TestDataGenerator;
public enum ProcessingLocationStrategy
{
    OnlyProcessingLocations,
    OnlyRegularLocations,
    Both
}
    
public enum MarkingStrategy
{
    None,
}
public class LocationGenerator :  Generator<Location>
{
    private readonly IEnumerable<string> _partTypes;
    private readonly ProcessingLocationStrategy strategy;
    private readonly MarkingStrategy markingGenerationStrategy;



    public LocationGenerator(IEnumerable<string> partTypes, ProcessingLocationStrategy processingLocationStrategy = ProcessingLocationStrategy.Both, MarkingStrategy markingStrategy = MarkingStrategy.None ):base(new Faker<Location>())
    {
        _partTypes = partTypes;
        this.strategy = processingLocationStrategy;
        this.markingGenerationStrategy = markingStrategy;
    }

    public override Location GenerateSingle()
    {
        var intervals = _partTypes.Select(e =>
        {
            var r = new Random();
            var min = r.Next(0, 7);
            var max = r.Next(min + 1, 10);
            return new Invariant(e, min, max);
        });

        var isProc = ValueSelection.Random.Bool();
        if (strategy == ProcessingLocationStrategy.OnlyProcessingLocations)
            isProc = true;
        if (strategy == ProcessingLocationStrategy.OnlyRegularLocations)
            isProc = false;
        
        
        return new Location(ValueSelection.Name.FirstName(), new Random().Next(1, 10), intervals, isProc);
    }
    
    
}