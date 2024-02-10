using System.Text;

namespace TapaalParser.TapaalGui.Writers;

internal abstract class TacpnUiXmlWriter<T> : ITacpnUiWriter
{
    protected readonly StringBuilder stringBuilder;
    protected T Parseable { get; set; }
    public abstract void AppendToStringBuilder();

    public TacpnUiXmlWriter(T value)
    {
        this.Parseable = value;
        this.stringBuilder = new StringBuilder();
    }

    public void Append(string text) => stringBuilder.Append(text);


    public override string ToString()
    {
        return stringBuilder.ToString();
    }
}