using Bogus;

namespace TestDataGenerator;

public interface IGenerator<T> where T : class
{
    public IEnumerable<T> GeneratedEntities { get; }
    public IEnumerable<T> Generate(int n)
    {
        List<T> result = [];
        for (int i = 0; i < n; i++)
        {
            result.Add(GenerateSingle());
        }

        return result;
    }
    public abstract T GenerateSingle();
    protected  Faker<T> Faker { get; }
    protected Faker ValueSelection { get; }
    protected Random Random { get; }
    public void UseSeed(int seed)
    {
        this.Faker.UseSeed(seed);
    }
    
}