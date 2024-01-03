namespace TACPN.Net;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> NotOfType<T>(this IEnumerable<T> source, Type type)
    {
        return source.Where(item => !type.IsInstanceOfType(item));
    }
}