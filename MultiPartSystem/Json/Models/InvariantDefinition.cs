using Common;
using Newtonsoft.Json;

namespace Tmpms.Common.Json.Models;

public record InvariantDefinition(string Part, int Min, int Max)
{
    private InvariantDefinition((string name, int minValue, int maxValue) tuple) : this(tuple.name, tuple.minValue,
        tuple.maxValue)
    {
    }

    [JsonConstructor]
    public InvariantDefinition(string part, string min, string max) : this(ParseMinMax(part, min, max))
    {
    }


    private static int ParseMax(string name, string min, string max)
    {
        var maxLower = max.ToLower();
        if (int.TryParse(max, out var maxValue))
            return maxValue;

        if (maxLower == "inf" || maxLower == "infty" || maxLower == "infinity")
            return InfinityInteger.Positive;
        throw new ArgumentException($"Values has wrong format for {name}: {min}, {max}");
    }


    private static (string name, int minValue, int maxValue) ParseMinMax(string name, string min, string max)
    {
        var minValue = int.Parse(min);
        var maxValue = ParseMax(name, min, max);
        return (name, minValue, maxValue);
    }
}