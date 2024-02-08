using TACPN;

namespace TapaalParser.TapaalGui.Writers;

internal class TopLevelWriter : TacpnUiXmlWriter<TimedArcColouredPetriNet>, ITacpnUiWriter
{
    public TopLevelWriter(TimedArcColouredPetriNet value) : base(value)
    {
    }

    public override void AppendToStringBuilder()
    {
        stringBuilder.Append($@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?> <pnml xmlns=""http://www.informatik.hu-berlin.de/top/pnml/ptNetb"">");
        stringBuilder.Append($"  <declaration>\n    <structure>\n      <declarations>");
    }
}