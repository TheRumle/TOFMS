using TapaalParser.TapaalGui.Placable;

namespace TapaalParser.TapaalGui;

public interface IGuiTranslater<TFrom>
{
    public string ToGuiElement(Placement<TFrom> placement);

}