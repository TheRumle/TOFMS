using TACPN.Colours.Type;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter.TransitionFactory;

public class MoveActionTransitionFactory : ITransitionFactory
{
    public Transition CreateTransition(MoveAction moveAction)
    {
        //TODO transition factory: Create transition guards
        
        
        
        
        
        return new Transition(moveAction.Name, ColourType.TokenAndDefaultColourType, TransitionGuard.Empty());
    }
}