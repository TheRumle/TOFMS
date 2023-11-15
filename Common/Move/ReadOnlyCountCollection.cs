using System.Collections;

namespace Common.Move;

public class ReadOnlyCountCollection<TKey> : IEnumerable<KeyValuePair<TKey, int>>
    where TKey : notnull
{
    protected Dictionary<TKey, int> Dictionary;
    internal AddByKey<TKey> ByKeyAdder = new();

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

    public int GetCount(TKey key)
    {
        if (Dictionary.TryGetValue(key, out var result)) return result;
        return 0;
    }


    public IEnumerator<KeyValuePair<TKey, int>> GetEnumerator()
    {
        return Dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}