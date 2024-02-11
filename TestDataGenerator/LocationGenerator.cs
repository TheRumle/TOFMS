using System.Collections;
using Bogus;
using TACPN.Colours;
using Tmpms.Common;

namespace TestDataGenerator;

public interface IGenerator<T> where T : class
{
    IEnumerable<T> Generate(int n);
    T GenerateSingle();
}

public abstract class Generator<T> : IGenerator<T> where T : class
{
    protected readonly Faker<T> Faker;
    protected readonly Faker ValueSelection = new();
    protected Random Random = new();

    protected Generator(Faker<T> faker)
    {
        Faker = faker;
    }

    public IEnumerable<T> Generate(int n)
    {
        for (int i = 0; i < n; i++)
            yield return GenerateSingle();
    }
    public abstract T GenerateSingle();
}

public class LocationGenerator :  Generator<Location>
{
    private readonly IEnumerable<string> _partTypes;

    public LocationGenerator(IEnumerable<string> partTypes):base(new Faker<Location>())
    {
        _partTypes = partTypes;
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
        return new Location(ValueSelection.Name.FirstName(), new Random().Next(1, 10), intervals, ValueSelection.Random.Bool());
    }
}