using TACPN.Net;
using TACPN.Net.Transitions;
using Tofms.Common.Move;
using Tofms.Common.Translate;

namespace TACPN.Adapters.TofmToTacpnAdapter;

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

        _transitionAttachableFactory.AdaptFrom(moveAction).AttachToTransition(transition);
        _transitionAttachableFactory.AdaptEmptyAfter(moveAction).AttachToTransition(transition);
        _transitionAttachableFactory.AdaptTo(moveAction).AttachToTransition(transition);
        _transitionAttachableFactory.AdaptEmptyBefore(moveAction).AttachToTransition(transition);


        var colourTypes = transition.InvolvedPlaces.Select(e => e.ColourType).DistinctBy(e=>e.Name);
        return new PetriNetComponent
        {
            Colors = moveAction.PartsToMove.Select(e => e.Key),
            Transitions = new List<Transition> { transition },
            Places = transition.InvolvedPlaces.ToList(),
            ColourTypes = colourTypes,
            Name = moveAction.Name + "Component"
        };
    }

    public async Task<PetriNetComponent> TranslateAsync(MoveAction moveAction)
    {
        return await Task.Run(() => Translate(moveAction));
    }
}