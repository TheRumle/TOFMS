﻿namespace Common;

public static class ListExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
    {
        return list.OrderBy(_ => Random.Shared.Next());
    }
    
    public static IEnumerable<T> Shuffle<T>(this T[] list)
    {
        return list.OrderBy(_ => Random.Shared.Next());
    }

}