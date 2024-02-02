using TACPN.Transitions;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter;

public interface ITransitionFactory
{

    public Transition CreateTransition(MoveAction moveAction);
}