namespace TestDataGenerator;

public interface IGenerator<T> where T : class
{
    IEnumerable<T> Generate(int n);
    T GenerateSingle();
}