using TACPN;

namespace TapaalParser;

public interface ITacpnTranslater<T>
{
    Task<T> TranslateNet(TimedArcColouredPetriNet netComponent);
}