using Bogus;
using Tmpms.Common;

namespace TestDataGenerator;

public class LocationGenerator
{
    private readonly Faker<Location> _faker;
    private readonly IEnumerable<string> _partTypes;

    public LocationGenerator(IEnumerable<string> partTypes)
    {
        _faker = new Faker<Location>();
        _partTypes = partTypes;
    }

    public IEnumerable<Location> GenerateLocations(int n)
    {

        Faker f = new Faker();
        for (int i = 0; i < n; i++)
        {
            var intervals = _partTypes.Select(e =>
            {
                var r = new Random();
                var min = r.Next(0, 7);
                var max = r.Next(min + 1, 10);
                return new Invariant(e, min, max);
            });
            var location = new Location(f.Name.FirstName(), new Random().Next(1, 10), intervals, f.Random.Bool());
            yield return location;
        }
    }
    
}