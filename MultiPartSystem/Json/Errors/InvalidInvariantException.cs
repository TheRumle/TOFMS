using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Errors;

public class InvalidInvariantException : InvalidJsonTmpmsException
{

    private string _readableMessage { get; init; }

    public static InvalidInvariantException InfiniteProcessing(LocationDefinition l, IEnumerable<InvariantDefinition> invariants)
    {
        return new InvalidInvariantException()
        {
            _readableMessage =
                $"Location {l.Name} is a processing location and has invariants with max age infinity: {string.Join(',',invariants.Select(e => $"{e.Part}: [{e.Min}:infty]"))}."
        };
    }

    public InvalidInvariantException(string part, string _min, string _max)
    {

        _readableMessage =$"Invalid definition invariant for {part}: {_min} to {_max}. Is {_min} > {_max} and did you use infty/inf wrong?";
    }

    public InvalidInvariantException(InvariantDefinition invariant) : this(invariant.Part, invariant.Min.ToString(),
        invariant.Max.ToString())
    {
    }
    
    public InvalidInvariantException(InvariantDefinition invariant, string partType) : this(invariant.Part, invariant.Min.ToString(),
        invariant.Max.ToString())
    {
        _readableMessage =
            $"Invalid definition invariant for {partType}: The part type was not defined";
    }

    private InvalidInvariantException()
    {
    }

    public override string ErrorCategory { get; } = "Invalid invariant";

    public override string ToString()
    {
        return _readableMessage;
    }
}