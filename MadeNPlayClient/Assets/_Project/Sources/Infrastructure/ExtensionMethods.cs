using System;
using System.Collections.Generic;
using Unity.Netcode;

public static class ExtensionMethods
{
    public static void AddRange<T>(this NetworkList<T> list, IList<T> values)
        where T : unmanaged, IEquatable<T>
    {
        for (int i = 0; i < values.Count; i++)
        {
            list.Add(values[i]);
        }
    }

    public static List<TSource> ToList<TSource>(this NetworkList<TSource> source)
        where TSource : unmanaged, IEquatable<TSource>
    {
        if (source == null)
            throw new ArgumentNullException(nameof(TSource));

        var result = new List<TSource>();

        for (int i = 0; i < source.Count; i++)
        {
            result.Add(source[i]);
        }

        return result;
    }

    public static IEnumerable<TSource> Where<TSource>(this NetworkList<TSource> source, Func<TSource, bool> predicate)
        where TSource : unmanaged, IEquatable<TSource>
    {
        foreach (TSource element in source)
        {
            if (predicate(element))
                yield return element;
        }
    }

    public static TSource FirstOrDefault<TSource>(this NetworkList<TSource> source, Func<TSource, bool> predicate)
        where TSource : unmanaged, IEquatable<TSource>
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }
        if (predicate == null)
        {
            throw new ArgumentNullException("predicate");
        }
        foreach (TSource local in source)
        {
            if (predicate(local))
            {
                return local;
            }
        }
        return default(TSource);
    }
}