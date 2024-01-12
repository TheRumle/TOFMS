using System.Text;
using Common;
using Tmpms.Common;
using Tmpms.Common.Move;

namespace Xml;

internal class EndSubnetWriter : SubnetDeclarer
{
    private readonly Location _endLocation;
    private readonly IEnumerable<Location> _locations;
    private readonly MoveAction _moveAction;


    public EndSubnetWriter(MoveAction moveAction, StringBuilder stringBuilder, JourneyCollection journeys, IEnumerable<string> parts) : base(stringBuilder, journeys)
    {
        this._locations = moveAction.InvolvedLocations.Where(e=>e.Name!= Location.EndLocationName);
        
        var endLocInvariants = parts.Select(e => new Invariant(e, 0, Infteger.PositiveInfinity));
        this._endLocation = new Location(Location.EndLocationName, Infteger.PositiveInfinity, endLocInvariants, true);
        this._moveAction = moveAction;

        
        if (moveAction.To.Name != Location.EndLocationName)
            throw new ArgumentException(nameof(moveAction.To) + " was not named end!");
        if (!moveAction.To.IsProcessing)
            throw new ArgumentException("End locations must be declared a processing location!");
   
    }

    public void WriteEndAction()
    {
        _stringBuilder.Append($@"<net active=""true"" id=""{_moveAction.Name}"" type=""P/T net"">");
        WriteCapacityLocation(_moveAction.From.ToCapacityLocation());
        WriteLocation(_moveAction.From);
        WriteEndLocation();

        TransitionWriter transitionWriter = new TransitionWriter(_stringBuilder, _moveAction);
        transitionWriter.WriteTransition(_journeys);

        ArcWriter writer = new ArcWriter(_stringBuilder, _moveAction, _journeys);
        
        writer.WriteTokenArcTo(_endLocation);
        writer.ProduceToCapPlace(_moveAction.From.ToCapacityLocation(),
            _moveAction.PartsToMove.Select(e => e.Value).Sum());
        
        writer.WriteArcFrom(_moveAction.From);
        _stringBuilder.Append("</net>");
    }

    public void WriteEndLocation()
    {
        _stringBuilder.Append($@"<place displayName=""true"" id=""{_endLocation.Name}"" initialMarking=""0"" invariant=""&lt; inf"" name=""{_endLocation.Name}"" nameOffsetX=""30"" nameOffsetY=""30"" positionX=""200"" positionY=""200"">
          <type>
            <text>{Colours.TokenColour}</text>
            <structure>
              <usersort declaration=""{Colours.TokenColour}""/>
            </structure>
          </type>");
        _stringBuilder.Append("</place>");
    }

}