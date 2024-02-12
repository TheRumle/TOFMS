using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;
using TmpmsPetriNetAdapter.ConditionGenerator;

namespace TmpmsPetriNetAdapter.TransitionFactory;

public class MoveActionTransitionFactory : ITransitionFactory
{
    private readonly TransitionGuardFactory _guardFactory;
    private readonly ColourType TransitionColourType;

    public MoveActionTransitionFactory(ColourTypeFactory colourTypeFactory)
    {
        this._guardFactory = new TransitionGuardFactory(colourTypeFactory);
        this.TransitionColourType = colourTypeFactory.Transitions;
    }
    
    public Transition CreateTransition(MoveAction moveAction)
    {
        var guard = _guardFactory.MoveActionGuard(moveAction);
        return new Transition(moveAction.Name, this.TransitionColourType, guard);
    }
}