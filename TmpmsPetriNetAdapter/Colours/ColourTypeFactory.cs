﻿using TACPN.Colours.Type;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.Colours;

public class ColourTypeFactory
{
    public readonly IntegerRangedColour Journey;
    public static readonly SingletonColourType DotColour = ColourType.DefaultColorType;
    public readonly ColourType Parts;
    public readonly ProductColourType Tokens;
    public const string JourneyName = "Journey"; 
    public const string TokenName = "Tokens"; 
    public const string PartsName = "Parts"; 
    
    public ColourTypeFactory(IEnumerable<string> parts, JourneyCollection journeyCollection)
    {
        Parts =  new ColourType("Parts", parts);
        Journey = new IntegerRangedColour(JourneyColourName, journeyCollection.JourneyLengths().MaxBy(e=>e.Value).Value);
        Tokens = new ProductColourType("Tokens", Parts, Journey);
    }

    private readonly string JourneyColourName = "Journey";
}