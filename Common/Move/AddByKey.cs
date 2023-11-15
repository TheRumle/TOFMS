namespace Common.Move;

internal class AddByKey<TKey> where TKey : notnull
{
    internal int AddToDict(Dictionary<TKey, int> dictionary, TKey key)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = dictionary.GetValueOrDefault(key) + 1;
            return dictionary[key];
        }
        
        dictionary.Add(key, 1);
        return 1;
    }
}