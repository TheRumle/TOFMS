using TACPN;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class MovingProductsOutOfLocationAttacherTest : Adapter
{

    public MovingProductsOutOfLocationAttacherTest(MoveAction moveAction, ColourTypeFactory ctFactory, IndexedJourneyCollection collection) 
        : base(ctFactory, collection)
    {
        FromLocation = moveAction.From;
        ToConsume = moveAction.PartsToMove;
        (FromPlace, FromPlaceHat) = LocationTranslator.CreatePlaceAndCapacityPlacePair(moveAction.From, collection, PartColourType);
    }

    private Location FromLocation { get; set; }

    private IEnumerable<KeyValuePair<string, int>> ToConsume { get; set; }

    private CapacityPlace FromPlaceHat { get; set; }

    private Place FromPlace { get; set; }

    public override void AttachToTransition(Transition transition)
    {
        AdaptPlace(transition);
        AdaptCapacityPlace(transition);
    }   

    private void AdaptCapacityPlace(Transition transition)
    {
        var consProdAmount = ToConsume.Sum(e => e.Value);
        var expression = new ColourExpression(Colour.DefaultTokenColour, ColourType.DefaultColorType, consProdAmount);
        //restore capacity by producing more capacity
        transition.AddOutGoingTo(FromPlaceHat, expression );
    }

    private void AdaptPlace(Transition transition)
    {
        var guards = ToConsume.Select(pair =>
        {
            var first = FromLocation.Invariants.First(e => e.PartType == pair.Key);
            return ColourTimeGuard.TokensGuard(first.Min, first.Max,this.PartColourType);
        });
        
        var a = JourneyColourExpressionFactory.CreatePartMoveTuple(ToConsume, FromPlace, Collection);
        transition.AddInGoingFrom(FromPlace, guards,a);
    }
}