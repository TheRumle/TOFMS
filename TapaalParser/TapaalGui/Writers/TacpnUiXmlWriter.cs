using System.Text;

namespace TapaalParser.TapaalGui.Writers;

public abstract class TacpnUiXmlWriter<T> : ITacpnUiWriter
{
    private readonly StringBuilder stringBuilder;
    protected T Parseable { get; set; }
    public abstract void AppendAllText();

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