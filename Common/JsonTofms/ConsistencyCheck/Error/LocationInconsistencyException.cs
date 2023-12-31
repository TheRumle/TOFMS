﻿using Tofms.Common.JsonTofms.Models;

namespace Tofms.Common.JsonTofms.ConsistencyCheck.Error;

public abstract class LocationInconsistencyException : InvalidJsonTofmException
{
    public LocationInconsistencyException(LocationDefinition location)
    {
        First = location;
    }

    public LocationDefinition First { get; set; }

    public abstract override string ToString();
}