﻿namespace Tmpms.Common.JsonTofms.Models;

public record LocationDefinition(string Name, int Capacity, List<InvariantDefinition> Invariants, bool IsProcessing);