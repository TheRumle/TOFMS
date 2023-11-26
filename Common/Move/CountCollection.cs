namespace Tofms.Common.Move;

public class CountCollection<TKey> : ReadOnlyCountCollection<TKey> where TKey : notnull
{
    public CountCollection(IEnumerable<TKey> partsToMove) : base(partsToMove)
    {
    }

    public CountCollection(IEnumerable<KeyValuePair<TKey, int>> pairs) : base(pairs)
    {
    }

    public CountCollection(KeyValuePair<TKey, int> pair) : base(new[] { pair })
    {
    }

    public CountCollection()
    {
    }

    public int AddKey(TKey key)
    {
        return ByKeyAdder.AddToDict(Dictionary, key);
    }
}