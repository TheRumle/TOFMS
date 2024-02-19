using Bogus;
using Tmpms;

namespace TestDataGenerator;

public class PartGenerator : IGenerator<Part>
{
    private readonly int _minAge;
    private readonly int _maxAge;
    private readonly List<string> _partTypes;
    private List<Part> _generated = new();

    public PartGenerator(IEnumerable<string> partTypes, int minAge, int maxAge)
    {
        _minAge = minAge;
        _maxAge = maxAge+1;
        _partTypes = partTypes.ToList();
    }

    public IEnumerable<Part> GeneratedEntities => _generated;
    public Part GenerateSingle() => Create([]);
    public Part GenerateSingle(int age) => Create([], age);
    public Part GenerateSingle(int age, IEnumerable<Location> locations) => Create(locations, age);

    private Part Create(IEnumerable<Location> journey, int age)
    {
        _generated.Add(new Part(_partTypes[Random.Next(_partTypes.Count)], age, journey));
        return _generated.Last();
    }

    private Part Create(IEnumerable<Location> journey)
    {
        return Create(journey, Random.Next(_minAge, _maxAge));
    }

    public Part GenerateSingle(IEnumerable<Location> journey) => Create(journey);

    public Faker<Part> Faker { get; } = new();
    public Faker ValueSelection { get; } = new();
    public Random Random { get; } = new ();
}