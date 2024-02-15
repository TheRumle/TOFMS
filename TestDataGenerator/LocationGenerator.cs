using Bogus;
using Tmpms;

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
public class LocationGenerator :  IGenerator<Location>
{
    private readonly IEnumerable<string> _partTypes;
    private readonly ProcessingLocationStrategy strategy;
    private readonly MarkingStrategy markingGenerationStrategy;
    private readonly List<Location> _generated;

    public LocationGenerator(IEnumerable<string> partTypes, ProcessingLocationStrategy processingLocationStrategy = ProcessingLocationStrategy.Both, MarkingStrategy markingStrategy = MarkingStrategy.None )
    {
        _partTypes = partTypes;
        this.strategy = processingLocationStrategy;
        this.markingGenerationStrategy = markingStrategy;
        this._generated = new List<Location>();
    }

    public IEnumerable<Location> GeneratedEntities => _generated;

    public Location GenerateSingle()
    {
        return GenerateSingle(strategy);
    }

    public Faker<Location> Faker { get; } = new();
    public Faker ValueSelection { get; } = new();
    public Random Random { get; } = new();

    public Location GenerateSingle(ProcessingLocationStrategy localStrategy)
    {
        var intervals = _partTypes.Select(e =>
        {
            var r = new Random();
            var min = r.Next(0, 7);
            var max = r.Next(min + 1, 10);
            return new Invariant(e, min, max);
        });

        var isProc = ValueSelection.Random.Bool();
        if (localStrategy == ProcessingLocationStrategy.OnlyProcessingLocations)
            isProc = true;
        if (localStrategy == ProcessingLocationStrategy.OnlyRegularLocations)
            isProc = false;
        
        
        return GetValue(intervals, isProc);
    }

    private Location GetValue(IEnumerable<Invariant> intervals, bool isProc)
    {
        var value = new Location(ValueSelection.Name.FirstName(), new Random().Next(1, 10), intervals, isProc, _partTypes);
        _generated.Add(value);
        return value;
    }

    public IEnumerable<Location> Generate(int n, ProcessingLocationStrategy strategy)
    {
        List<Location> l = [];
        for (int i = 0; i < n; i++)
        {
            l.Add(GenerateSingle(strategy));
        }

        return l;
    }
    
    public IEnumerable<Location> Generate(int n)
    {
        List<Location> l = [];
        for (int i = 0; i < n; i++)
        {
            l.Add(GenerateSingle(strategy));
        }

        return l;
    }
}