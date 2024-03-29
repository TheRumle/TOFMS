﻿using System.Text.RegularExpressions;
using FluentAssertions;
using TapaalParser;
using TapaalParser.TapaalGui;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter;
using TmpmsPetriNetAdapter.Colours;
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
        var indexed = System.Journeys.ToIndexedJourney();
        
        
        var colourFactory = new ColourTypeFactory(System.Parts, System.Journeys);
        var variableFactory = new ColourVariableFactory(colourFactory);
        
        var arcFactory = new MoveActionToArcsFactory(System.Journeys, colourFactory);
        var transitionFactory = new MoveActionTransitionFactory(colourFactory, variableFactory);
        var translater = new TofmToTacpnTranslater(arcFactory, transitionFactory, indexed);
        IEnumerable<PetriNetComponent> petriNetComponents = System.MoveActions.Select(e => translater.Translate(e));
        
        TimedArcColouredPetriNet a = new TimedArcColouredPetriNet(petriNetComponents.ToArray())
        {
            Name = "Net",
            ColourTypes = [colourFactory.DotColour, colourFactory.Parts,colourFactory.Journey, colourFactory.Tokens],
            Variables = variableFactory.CreatedVariables
        };
        
        var netText = await GuiTranslater.TranslateNet(a);
        return Regex.Replace(netText, @"\s+", "");
    }

    [Fact]
    public async Task ShouldComplyWithOldTranslation()
    {
        var oldText = DirectlyTranslatedText();
        var newText = await NewTranslatedText();
        newText.Should().Be(oldText);
    }
    
    [Fact] public void ShouldLoadAndParseSystem(){}
}