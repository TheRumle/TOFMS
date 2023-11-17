using Common.Move;
using Common.Translate;
using TACPN.Adapters.Location;
using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN.Adapters;

public class TofmToTacpnTranslater : IMoveActionTranslation<PetriNetComponent>
{
    private readonly ITransitionAttachableFactory _transitionAttachableFactory;

    public TofmToTacpnTranslater(ITransitionAttachableFactory transitionAttachableFactory)
    {
        _transitionAttachableFactory = transitionAttachableFactory;
    }

    public PetriNetComponent Translate(MoveAction moveAction)
    {
        var transition = new Transition(moveAction.Name);
        var adapters = new[]
        {
            _transitionAttachableFactory.AdaptEmptyAfter(moveAction),
            _transitionAttachableFactory.AdaptEmptyBefore(moveAction),
            _transitionAttachableFactory.AdaptFrom(moveAction),
            _transitionAttachableFactory.AdaptTo(moveAction)
        };
        
        foreach (var adapter in adapters)
            adapter.AttachToTransition(transition);

        return new PetriNetComponent
        {
            Colors = moveAction.PartsToMove.Select(e => e.Key),
            Transitions = new List<Transition> { transition },
            Places = new List<Place>()
        };  
    }

    public Task<PetriNetComponent> TranslateAsync(MoveAction moveAction)
    {
        return Task.Run(() => Translate(moveAction));
    }
}