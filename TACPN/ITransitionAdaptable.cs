using TACPN.Net.Transitions;

namespace TACPN;

public interface ITransitionAdaptable
{
    public void AdaptToTransition(Transition transition);
}