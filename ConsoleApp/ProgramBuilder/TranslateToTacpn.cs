using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;
using Tofms.Common.Move;

namespace ConsoleApp.ProgramBuilder;

public class TranslateToTacpn
{
    private readonly IEnumerable<MoveAction> moveActions;
    public readonly ParseAndValidateTofmSystem PrevStep;

    public TranslateToTacpn(IEnumerable<MoveAction> moveActions, ParseAndValidateTofmSystem parseAndValidateTofmSystem)
    {
        this.moveActions = moveActions;
        this.PrevStep = parseAndValidateTofmSystem;
    }

    public ExtractTacpnXmlFormat TranslateTofmsToTacpnComponents()
    {
        TofmToTacpnTranslater translater = new TofmToTacpnTranslater(new MoveActionToTransitionFactory());
        var components = moveActions
            .Select( e=> translater.TranslateAsync(e))
            .Select(e=>
            {
                e.Wait();
                return e.Result;
            });

        return new ExtractTacpnXmlFormat(components, this);
    }
}