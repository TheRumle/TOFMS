using TACPN.Colours.Type;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.ConditionGenerator;

namespace TmpmsPetriNetAdapter.TransitionFactory;

public class MoveActionTransitionFactory : ITransitionFactory
{
    private readonly TransitionGuardFactory _guardFactory;
    private readonly ColourType _partsColourType;

    public MoveActionTransitionFactory(JourneyCollection journeysForParts, ColourType partsColourType)
    {
        this._guardFactory = new TransitionGuardFactory(journeysForParts, partsColourType);
        this._partsColourType = partsColourType;
    }
    
    public Transition CreateTransition(MoveAction moveAction)
    {
        var guard = _guardFactory.MoveActionGuard(moveAction);
        return new Transition(moveAction.Name, ColourType.TokensColourType(_partsColourType), guard);
    }
}