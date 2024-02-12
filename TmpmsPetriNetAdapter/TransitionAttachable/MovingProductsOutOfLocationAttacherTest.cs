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
    private Location FromLocation { get; set; }
    private IEnumerable<KeyValuePair<string, int>> ToConsume { get; set; }
    private Place FromPlaceHat { get; set; }
    private Place FromPlace { get; set; }
    
    public MovingProductsOutOfLocationAttacherTest(MoveAction moveAction, ColourTypeFactory ctFactory, JourneyCollection collection) 
        : base(ctFactory)
    {
        FromLocation = moveAction.From;
        ToConsume = moveAction.PartsToMove;
        (FromPlace, FromPlaceHat) = PlaceFactory.CreatePlaceAndCapacityPlacePair(moveAction.From);
    }



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
            var invariant = FromLocation.Invariants.First(e => e.PartType == pair.Key);
            return TimeGuardFactory.TokensGuard(invariant.Min, invariant.Max);
        });

        if (FromLocation.IsProcessing)
        {
            var expression = JourneyColourExpressionFactory.CreatePartJourneyUpdate(ToConsume);
            transition.AddInGoingFrom(FromPlace, guards,expression);
        }
        else
        {
            var expression = JourneyColourExpressionFactory.CreatePartMoveTuple(ToConsume);
            transition.AddInGoingFrom(FromPlace, guards,expression);
        }
    }
}