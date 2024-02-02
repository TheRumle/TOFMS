using System.Security.Cryptography.X509Certificates;
using TACPN;

namespace TapaalParser.TapaalGui;

public class TapaalTacpnGuiParser : ITacpnTranslater<string>
{
    public Task<string> TranslateNet(TimedArcColouredPetriNet petriNet)
    {
        //TODO WRITE THE TOP LEVEL SHIZZLE
        
        
        

        return Task.FromResult("HELO");
        throw new NotImplementedException();
    }
}