using System.Collections;

namespace Tofms.Common.Move;

public class ReadOnlyCountCollection<TKey> : IEnumerable<KeyValuePair<TKey, int>>
    where TKey : notnull
{
    internal AddByKey<TKey> ByKeyAdder = new();
    protected Dictionary<TKey, int> Dictionary;

    public ReadOnlyCountCollection(IEnumerable<TKey> keys)
    {
        Dictionary = new Dictionary<TKey, int>();
        foreach (var key in keys) ByKeyAdder.AddToDict(Dictionary, key);
    }

    public ReadOnlyCountCollection(IEnumerable<KeyValuePair<TKey, int>> kvpsValuePairs)
    {
        Dictionary = new Dictionary<TKey, int>(kvpsValuePairs);
    }

    public ReadOnlyCountCollection()
    {
        Dictionary = new Dictionary<TKey, int>();
    }


    public IEnumerator<KeyValuePair<TKey, int>> GetEnumerator()
    {
        return Dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int GetCount(TKey key)
    {
        if (Dictionary.TryGetValue(key, out var result)) return result;
        return 0;
    }
}