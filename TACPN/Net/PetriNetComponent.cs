﻿using TACPN.Net.Transitions;

namespace TACPN.Net;

public class PetriNetComponent
{
    public required ICollection<Transition> Transitions { get; init; }
    public required ICollection<CapacityPlace> CapacityPlaces { get; init; }
    public required ICollection<Place> Places { get; init; }

    public  HashSet<IPlace> AllPlaces()
    {
        var a = new HashSet<IPlace>(Places);
        a.UnionWith(CapacityPlaces);
        return a;
    }
    public required IEnumerable<string> Colors { get; init; }

    public required IEnumerable<ColourType> ColourTypes { get; init; }
    public required string Name { get; init; }
}