using System.Collections.Concurrent;
using Tofms.Common.JsonTofms.Models;

namespace JsonFixtures.Tofms.Fixtures;

public class AllComponentsFixture : JsonTofmsFixture
{
    public AllComponentsFixture()
    {
        ReadAndCreateComponents(out var types, out var components);
        Components = components;
        PartTypes = types;
    }

    public List<string> PartTypes { get; }

    public List<TofmComponent> Components { get; }



    public TofmSystem AsTofmSystem()
    {
        return new TofmSystem
        {
            Components = Components,
            Parts = PartTypes
        };
    }


    protected static void ReadAndCreateComponents(out List<string> parts, out List<TofmComponent> tofmComponents)
    {
        var partTypes = new ConcurrentQueue<string>();
        var components = new ConcurrentQueue<TofmComponent>();
        var files = Directory.EnumerateFiles(ValidComponentPath).Where(e => e.ToLower().EndsWith(".json"));
        Parallel.ForEach(files, file => { ReadComponent(file, partTypes, components); });

        parts = new List<string>(partTypes);
        tofmComponents = new List<TofmComponent>(components);
    }


}