using System.Collections.ObjectModel;
using TACPN.Net;
using TACPN.Net.Arcs;

namespace TapaalParser.TapaalGui;

public static class TaskExtensions {
    public static void Add<T>(this ICollection<Task<T>> tasks, Func<T> action)
    {
        var t = Task.Run(action.Invoke);
        tasks.Add(t);
    }

}

public static class ProductionExtensions
{
    public static TokenCollection ToTokenCollection(this IEnumerable<Production> productions)
    {
        var enumerable = productions as Production[] ?? productions.ToArray();
        var tokens = enumerable.SelectMany(e => Enumerable.Repeat(new Token(e.Color), e.Amount));
        return new TokenCollection(tokens)
        {
            Colours = enumerable.Select(e=>e.Color).ToHashSet().ToList()
        };
    } 
}