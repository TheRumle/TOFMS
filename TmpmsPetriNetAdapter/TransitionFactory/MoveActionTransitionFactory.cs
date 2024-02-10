using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;
using TmpmsPetriNetAdapter.ConditionGenerator;

namespace TmpmsPetriNetAdapter.TransitionFactory;

public class MoveActionTransitionFactory : ITransitionFactory
{
    private readonly TransitionGuardFactory _guardFactory;
    private readonly ColourType _tokensColourType;

    public MoveActionTransitionFactory(IndexedJourneyCollection journeysForParts, ColourTypeFactory colourTypeFactory)
    {
        
        this._guardFactory = new TransitionGuardFactory(journeysForParts, colourTypeFactory.Parts, new ColourVariableFactory(colourTypeFactory));
        this._tokensColourType = colourTypeFactory.Tokens;
    }
    
    public Transition CreateTransition(MoveAction moveAction)
    {
        var guard = _guardFactory.MoveActionGuard(moveAction);
        return new Transition(moveAction.Name, this._tokensColourType, guard);
    }
}