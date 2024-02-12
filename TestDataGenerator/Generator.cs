﻿using Bogus;

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
        List<T> result = [];
        for (int i = 0; i < n; i++)
        {
            result.Add(GenerateSingle());
        }

        return result;
    }
    public abstract T GenerateSingle();

    public void UseSeed(int seed)
    {
        this.Faker.UseSeed(seed);
    }
}