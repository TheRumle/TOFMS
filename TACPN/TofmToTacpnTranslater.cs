using Common.Move;
using Common.Translate;
using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN;

public class TofmToTacpnTranslater : IMoveActionTranslation<PetriNetComponent>
{
    private async Task<Transition> ExtractTransition(string name)
    {
        return await Task.Run(() =>
        {
            return new Transition(name);
        });
    }
    
    public PetriNetComponent Translate(MoveAction moveAction)
    {
        var name = moveAction.Name;


            return null;
    }

    public Task<PetriNetComponent> TranslateAsync(MoveAction moveAction)
    {
        return Task.Run(PetriNetComponent.Empty);
    }
}