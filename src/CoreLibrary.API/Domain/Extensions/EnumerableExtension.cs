using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Extensions;

[ExcludeFromCodeCoverage]
public static class EnumerableExtension
{
    public static bool TryGetFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, [MaybeNullWhen(false)] out TSource data)
    {
        foreach (TSource element in source)
        {
            if (!predicate(element))
                continue;

            data = element;
            return true;
        }

        data = default;
        return false;
    }

    public static bool ContainsAny(this string compare, params string[] comparers)
    {
        foreach (var comparer in comparers)
            if (compare.Contains(comparer)) return true;
        return false;
    }

    public static bool TryGetString<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, [MaybeNullWhen(false)] out string value) where TKey : notnull
    {
        var status = dict.TryGetValue(key, out TValue? result);
        value = status ? result?.ToString() : null;
        return status;
    }

    /// <summary>
    /// Returns the index of the first occurrence in a sequence by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="list">A sequence in which to locate a value.</param>
    /// <param name="value">The object to locate in the sequence</param>
    /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
    public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value) where TSource : IEquatable<TSource>
    {
        return list.IndexOf(value, EqualityComparer<TSource>.Default);
    }

    /// <summary>
    /// Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="list">A sequence in which to locate a value.</param>
    /// <param name="value">The object to locate in the sequence</param>
    /// <param name="comparer">An equality comparer to compare values.</param>
    /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
    public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
    {
        int index = 0;
        foreach (var item in list)
        {
            if (comparer.Equals(item, value))
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    public static IEnumerable<T>? ForEach<T>(this IEnumerable<T>? list, Action<T> action)
    {
        if (list is null || !list.Any()) return list;
        using (IEnumerator<T> enumerator = list.GetEnumerator())
            while (enumerator.MoveNext())
                action(enumerator.Current);
        return list;
    }

    public static ICollection<KeyValuePair<TKey, TValue>> AddThis<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> dict, TKey key, TValue value) where TKey : notnull
    {
        if (!dict.Any(a => a.Key.Equals(key)))
            dict.Add(new KeyValuePair<TKey, TValue>(key, value));
        return dict;
    }

    public static ICollection<KeyValuePair<TKey, TValue>> AddThese<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> dict, ICollection<KeyValuePair<TKey, TValue>> keyValues) where TKey : notnull
    {
        keyValues.ForEach(keyValue =>
        {
            if (!dict.Any(a => a.Key.Equals(keyValue.Key)))
                dict.Add(new KeyValuePair<TKey, TValue>(keyValue.Key, keyValue.Value));
        });
        return dict;
    }

    public static ICollection<KeyValuePair<TKey, TValue>> AddThese<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> dict, params KeyValuePair<TKey, TValue>[] keyValues) where TKey : notnull
    {
        keyValues.ForEach(keyValue =>
        {
            if (!dict.Any(a => a.Key.Equals(keyValue.Key)))
                dict.Add(new KeyValuePair<TKey, TValue>(keyValue.Key, keyValue.Value));
        });
        return dict;
    }

    public static IEnumerable<TResult> SelectSync<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TResult> selector)
    {
        var list = new List<TResult>();
        foreach (var item in source)
            list.Add(selector(item));
        return list;
    }

    public static bool IsEqualsAny<T>(this T item, params T[] comparers) =>
        comparers.Any(a => item != null && item.Equals(a));
}
