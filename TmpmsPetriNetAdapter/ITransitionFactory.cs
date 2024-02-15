using TACPN.Transitions;
using Tmpms.Move;

namespace TmpmsPetriNetAdapter;

public interface ITransitionFactory
{

    public Transition CreateTransition(MoveAction moveAction);
}