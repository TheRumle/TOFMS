using System.Text.RegularExpressions;
using FluentAssertions;
using TapaalParser;
using TapaalParser.TapaalGui;
using Tmpms.Common;
using TmpmsPetriNetAdapter;
using TmpmsPetriNetAdapter.TransitionAttachable;
using TmpmsPetriNetAdapter.TransitionFactory;
using Xml;
using Xunit;

namespace TACPN.IntegrationTests.DirectCompliance;

public abstract class GuiTranslationAdherenceTest
{
    protected abstract string TestName { get; }
    protected ITacpnTranslater<string> GuiTranslater { get; } = new TapaalTacpnGuiParser();
    protected abstract TimedMultipartSystem System { get; }
    
    public string DirectlyTranslatedText()
    {
        var directlyParsedString = new TmpmsDirectParser(System).Parse();
        var whiteSpaceRemovedDirectlyParsedString = Regex.Replace(directlyParsedString, @"\s+", "");
        return whiteSpaceRemovedDirectlyParsedString;
    }

    public async Task<string> NewTranslatedText()
    {
        var arcFactory = new MoveActionToArcsFactory(System.Journeys.ToIndexedJourney());
        var transitionFactory = new MoveActionTransitionFactory();
        var translater =
            new TofmToTacpnTranslater(arcFactory, transitionFactory, System.Journeys.ToIndexedJourney());

        IEnumerable<PetriNetComponent> petriNetComponents = System.MoveActions.Select(e => translater.Translate(e));
        TimedArcColouredPetriNet a = new TimedArcColouredPetriNet(petriNetComponents.ToArray())
        {
            Name = "Net",
            ColourTypes = [],
        };
        
        var netText = await GuiTranslater.TranslateNet(a);
        return Regex.Replace(netText, @"\s+", "");
    }

    [Fact]
    public async Task ShouldComplyWithOldTranslation()
    {
        (await NewTranslatedText()).Should().Be(DirectlyTranslatedText());
    }
    
    [Fact] public void ShouldLoadAndParseSystem(){}
}