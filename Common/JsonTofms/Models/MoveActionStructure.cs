namespace Common.JsonTofms.Models;

public record MoveActionStructure(string Name, List<Tuple<int, string>> Parts, string From, string To, List<string> EmptyBefore, List<string> EmptyAfter);