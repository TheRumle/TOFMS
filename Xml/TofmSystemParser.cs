using System.Security.Cryptography.X509Certificates;
using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Net;
using Tofms.Common;
using Tofms.Common.Move;

namespace Xml;

public class TofmSystemParser
{
    private ValidatedTofmSystem _system;
    private readonly List<string> _partnames;
    private readonly IEnumerable<Location> _locations;
    private readonly StringBuilder _stringBuilder;
    private readonly JourneyCollection _journeys;
    private readonly CapacityLocation[] _capacityLocations;
    private readonly IEnumerable<MoveAction> _moveactions;
    private readonly HashSet<Invariant> _invariants;

    public TofmSystemParser(ValidatedTofmSystem system)
    {
        _system = system;
        this._partnames = system.Parts.ToList();
        _locations = GetLocations();
        this._stringBuilder = new StringBuilder();
        this._journeys = CreateJourneyCollection();
        this._capacityLocations = _locations.Where(e=>e.Name != Location.EndLocationName).Select(e => e.ToCapacityLocation()).ToArray();
        this._moveactions = system.MoveActions;
        this._invariants = system.MoveActions.SelectMany(e => e.InvolvedLocations.SelectMany(e => e.Invariants)).ToHashSet();

    }

    private IEnumerable<Location> GetLocations()
    {
        return _system.MoveActions.SelectMany(e=>e.InvolvedLocations).DistinctBy(e => e.Name);
    }

    private JourneyCollection CreateJourneyCollection()
    {
        var dict = new Dictionary<string, List<Location>>(_system.Journeys);
        IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> j = dict.Select(e =>
        {
            var k = e.Key!;
            var values = e.Value;
            IEnumerable<KeyValuePair<int, Location>> newValues =
                values.Select((h, index) => KeyValuePair.Create(index, h));
            return KeyValuePair.Create(k, newValues);
        });
        return new JourneyCollection(j);
    }


    public string Parse()
    {
        _stringBuilder.Append($@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?> <pnml xmlns=""http://www.informatik.hu-berlin.de/top/pnml/ptNetb"">");
        _stringBuilder.Append($"  <declaration>\n    <structure>\n      <declarations>");
        DeclareColours();
        DeclareVariables();
        _stringBuilder.Append($"      </declarations>\n    </structure>\n  </declaration>");
        DeclareLocations();
        DeclareComponents();

        _stringBuilder.Append($@"  <feature isColored=""true"" isGame=""false"" isTimed=""true""/> </pnml>");
        return _stringBuilder.ToString();
    }

    private void DeclareComponents()
    {
        foreach (var moveAction in _moveactions)
        {
            if (moveAction.To.Name != Location.EndLocationName)
            {
                SubnetDeclarer subnetDeclarer = new SubnetDeclarer(_stringBuilder, _journeys);
                subnetDeclarer.WriteComponent(moveAction, _capacityLocations);
            }
            else
            {
                EndSubnetWriter endWriter = new EndSubnetWriter(moveAction, _stringBuilder, _journeys);
                endWriter.WriteEndAction();
            }
            
        }
    }

 

    private void DeclareLocations()
    {
        SharedPlaceDeclarationWriter declarationWriter = new SharedPlaceDeclarationWriter(this._stringBuilder);
        var endLocInvariants = _system.Parts.Select(e => new Invariant(e, 0, Infteger.PositiveInfinity));
        var endLocation = new Location(Location.EndLocationName, Infteger.PositiveInfinity, endLocInvariants, true);
        var locs = new List<Location>(_locations.Count()+1) { endLocation };
        locs.AddRange(_locations);
        declarationWriter.WritePlaces(locs, _journeys);
        declarationWriter.WriteCapacityPlaces(_capacityLocations, _journeys);
        
        
        
    }

    private void DeclareVariables()
    {
        var variableDeclarationWriter = new VariableDeclarer(_stringBuilder);
        variableDeclarationWriter.WriteVariables(this._journeys);
    }

    private void DeclareColours()
    {
        var declarer = new ColourDeclarer(_stringBuilder);
        declarer.WriteDot();
        declarer.WriteParts(_partnames);
        declarer.WriteJourney(_stringBuilder, _journeys);
        declarer.WriteTokenDeclaration(_journeys);
    }
}

internal class EndSubnetWriter : SubnetDeclarer
{
    private readonly MoveAction moveAction;
    private readonly Location _end;
    private readonly IEnumerable<Location> locations;

    public EndSubnetWriter(MoveAction moveAction, StringBuilder stringBuilder, JourneyCollection journeys) : base(stringBuilder, journeys)
    {
        if (moveAction.To.Name != Location.EndLocationName)
            throw new ArgumentException(nameof(moveAction.To) + " was not named end!");
        if (!moveAction.To.IsProcessing)
            throw new ArgumentException("End locations must be declared a processing location!");
        
        this.moveAction = moveAction;
        this._end = moveAction.To;
        this.locations = moveAction.InvolvedLocations.Where(e => e.Name != Location.EndLocationName);
    }

    public void WriteEndAction()
    {
        _stringBuilder.Append($@"<net active=""true"" id=""{moveAction.Name}"" type=""P/T net"">");
        IEnumerable<CapacityLocation> involvedCapacityLocations = locations.Select(e => e.ToCapacityLocation());
        
        foreach (var capacityLocation in involvedCapacityLocations)
            WriteCapacityLocation(capacityLocation);

        foreach (var location in moveAction.InvolvedLocations)
            WriteLocation(location);

        WriteEndLocation();

        
        TransitionWriter transitionWriter = new TransitionWriter(_stringBuilder, moveAction);
        transitionWriter.WriteTransition(_journeys);
        ArcWriter arcWriter = new ArcWriter(_stringBuilder, moveAction, _journeys);
        arcWriter.WriteArcs();

        arcWriter.WriteArcTo(this._end);
        _stringBuilder.Append("</net>");
    }

    private void WriteEndLocation()
    {
        throw new NotImplementedException();
    }
}