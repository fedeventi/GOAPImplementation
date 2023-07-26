using System.Collections.Generic;
using System.Linq;
using System;

public static class DictionaryExtensions
{
    public static bool In<T>(this T x, HashSet<T> set)
    {
        return set.Contains(x);
    }
    
    public static bool In<K, V>(this KeyValuePair<K, V> x, Dictionary<K, V> dict)
    {
        return dict.Contains(x);
    }
    
    public static bool In(this KeyValuePair<StatesGoap, Func<PlanerValue, bool>> x, Dictionary<StatesGoap, PlanerValue> dict)
    {
        if (!dict.ContainsKey(x.Key))
            return false;
    
        return x.Value.Invoke(dict[x.Key]);
    }

    public static void UpdateWith<K, V>(this Dictionary<K, V> a, Dictionary<K, V> b)
    {
        foreach (var kvp in b)
        {
            a[kvp.Key] = kvp.Value;
        }
    }

    public static void UpdateWith(this Dictionary<StatesGoap, PlanerValue> a, Dictionary<StatesGoap, Action<PlanerValue>> b)
    {
        foreach (var kvp in b)
        {
            kvp.Value.Invoke(a[kvp.Key]);
        }
    }
}