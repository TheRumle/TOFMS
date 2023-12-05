﻿using Tofms.Common;

namespace TACPN.Net;

public interface IPlace
{
    string Name { get; }
    bool IsCapacityLocation { get;  }
    ColourType ColourType { get; }
    bool IsProcessingPlace { get; }
}

public interface IPlace<T> : IPlace
{
    IEnumerable<ColourInvariant<T>> ColourInvariants { get; }
}

public interface IPlace<T, TT> : IPlace
{
    IEnumerable<ColourInvariant<T,TT>> ColourInvariants { get; }
}

public class Place : IPlace<int, string>
{
    public string Name { get; set; }
    public bool IsCapacityLocation { get; set; }
    public ColourType ColourType { get; init; }
    public bool IsProcessingPlace { get; set;  }
    public IEnumerable<ColourInvariant<int, string>> ColourInvariants { get; set; }

    public Place(bool isProcessingPlace, string name, IEnumerable<ColourInvariant<int, string>> colourInvariantses, ColourType colourType)
    {
        IsProcessingPlace = isProcessingPlace;
        Name = name;
        ColourInvariants = colourInvariantses; 
        IsCapacityLocation = false;
        ColourType = colourType;

    }

    public override string ToString()
    {
        return this.Name;
    }
}

public record ColourInvariant(int MaxAge)
{
    public static ColourInvariant<string> DotDefault = new ColourInvariant<string>(ColourType.DefaultColorType, ColourType.DefaultColorType.Colours.First(), InfinityInteger.Positive);


}
public record ColourInvariant<T1>(ColourType ColourType, T1 FirstColour, int MaxAge ) : ColourInvariant(MaxAge)
{
    
}

public record ColourInvariant<T1,T2>(ColourType ColourType, T1 FirstColour, T2 SecondColour, int MaxAge) : ColourInvariant<T1>(ColourType, FirstColour, MaxAge) 

{



}