using Tmpms.Common;
using Tmpms.Common.Move;

namespace ConsoleApp.ProgramBuilder;

public class TranslateToTacpn
{
    public readonly TimedMultipartSystem TimedMultipartSystem;
    private readonly IEnumerable<MoveAction> _moveActions;
    private readonly IReadOnlyDictionary<string, IEnumerable<Location>> _journeys;
    public readonly ParseAndValidateTofmSystem PrevStep;

    public TranslateToTacpn(TimedMultipartSystem timedMultipartSystem,
        IReadOnlyDictionary<string, IEnumerable<Location>> journeys, ParseAndValidateTofmSystem parseAndValidateTofmSystem)
    {
        TimedMultipartSystem = timedMultipartSystem;
        this._moveActions = timedMultipartSystem.MoveActions;
        _journeys = journeys;
        this.PrevStep = parseAndValidateTofmSystem;
    }
}

