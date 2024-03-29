﻿using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.Transitions.Guard;


public interface IOrStatement
{
    ColourType ColourType { get; }
    void AddComparison(IColourComparison<ColourVariable, int> comparator);
    public IEnumerable<IColourComparison<ColourVariable, int>> Comparisons { get; }

    public string ToTapaalText();
}