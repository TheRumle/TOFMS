using TACPN;

namespace TapaalParser.TapaalGui;

public class TapaalTacpnGuiParser : ITacpnTranslater<string>
{
    
    public async Task<string> TranslateNet(TimedArcColouredPetriNet petriNet)
    {
        var writeAllTask = new WriteAllTask(petriNet)
            .WriteAll();

        return await writeAllTask;
    }
}