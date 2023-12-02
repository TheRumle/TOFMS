using TACPN.Net;
using TACPN.Net.Arcs;

namespace TapaalParser.TapaalGui;

public static class TaskExtensions {
    public static Task<T> Add<T>(this IEnumerable<Task<T>> tasks, Func<T> action)
    {
        return Task.Run(action.Invoke);
    }

}

public static class ProductionExtensions
{
    public static TokenCollection ToTokenCollection(this IEnumerable<Production> productions)
    {
        var enumerable = productions as Production[] ?? productions.ToArray();
        var tokens = enumerable.SelectMany(e => Enumerable.Repeat(new Token(e.Color, 0), e.Amount));
        return new TokenCollection(tokens)
        {
            Colours = enumerable.Select(e=>e.Color).ToHashSet().ToList()
        };
    } 
}