using TACPN;

namespace TapaalParser.TapaalGui.XmlWriters;

public static class TokenCollectionExtensions
{
    public static IOrderedEnumerable<(string Color, int Amount)> ColourAmounts(this TokenCollection collection)
    {
        return collection  
            .Colours
            .Where(e => collection.AmountOfColour(e) > 0)
            .Select(e => (Color: e, Amount: collection.AmountOfColour(e)))
            .OrderBy(e => e);
    }
    
    public static IOrderedEnumerable<(string Color, int Amount)> WithMoreThan0Occurances(this TokenCollection collection)
    {
        return collection  
            .Colours
            .Where(e => collection.AmountOfColour(e) > 0)
            .Select(e => (Color: e, Amount: collection.AmountOfColour(e)))
            .OrderBy(e => e);
    }
    

    
    
}