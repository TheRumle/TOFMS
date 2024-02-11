using Bogus;

namespace TestDataGenerator;

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