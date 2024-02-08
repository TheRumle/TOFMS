using System.Text;

namespace TapaalParser.TapaalGui.Writers;

internal abstract class TacpnUiXmlWriter<T> : ITacpnUiWriter
{
    protected readonly StringBuilder stringBuilder;

    public TacpnUiXmlWriter(T value)
    {
        this.Parseable = value;
        this.stringBuilder = new StringBuilder();
    }

    protected T Parseable { get; set; }
    public abstract void AppendToStringBuilder();

    public override string ToString()
    {
        return stringBuilder.ToString();
    }
}