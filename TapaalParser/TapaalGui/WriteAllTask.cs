using TACPN;
using TapaalParser.TapaalGui.Writers;

namespace TapaalParser.TapaalGui;

internal class WriteAllTask
{
    public readonly IEnumerable<ITacpnUiWriter> Writers;

    public WriteAllTask(TimedArcColouredPetriNet net)
    {
        Writers =
        [
            new TopLevelWriter(net),
            new ColourTypeDeclarationWriter(net.ColourTypes)
        ];
    }

    public async Task<string> WriteAll()
    {
        var actions = Writers.Select(writer => (Action)writer.AppendToStringBuilder)
            .Select(e => Task.Run(e.Invoke)).ToArray();

        await Task.WhenAll(actions);
        return Writers.Aggregate("", (s, writer) => s += writer.ToString());
    }

}