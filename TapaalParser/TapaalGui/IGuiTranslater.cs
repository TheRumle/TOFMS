namespace TapaalParser.TapaalGui;

internal interface IGuiTranslater<TFrom>
{
    public string XmlString(TFrom placement);

}