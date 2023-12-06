using System.Security.Cryptography.X509Certificates;
using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Net;
using Tofms.Common;
using Tofms.Common.JsonTofms.Models;
using TofmSystem = Tofms.Common.JsonTofms.Models.TofmSystem;

namespace Xml;

public class TofmSystemParser
{
    private TofmSystem _system;
    private readonly List<string> partnames;
    private readonly IEnumerable<LocationDefinition> locations;
    private readonly StringBuilder stringBuilder;
    private readonly JourneyCollection journey;

    public TofmSystemParser(TofmSystem system)
    {
        _system = system;
        this.partnames = system.Parts;
        this.locations = _system.Components.SelectMany(e => e.Locations).DistinctBy(e => e.Name);
        this.stringBuilder = new StringBuilder();

        IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> j = _system.Journeys.Select(e =>
        {
            var k = e.Key!;
            var values = e.Value;
            var newValues = values.Select(h => KeyValuePair.Create(values.IndexOf(h), h));
            return KeyValuePair.Create(k, newValues);
        });

        var journey = new JourneyCollection(j);

        this.journey = new JourneyCollection(j);
    }


    public string Parse()
    {
        
        DeclareColours(partnames);
        DeclareJourneyColours(_system.Journeys);
        DeclareDotColour();
        DeclareTokensColour();
        DeclareVariables(locations);
        
        DeclareLocations(locations);
        DeclareCapacityLocations(locations);
        
        foreach (var loc in locations)
        {
            Declare(loc);
            DeclareCapacityPlace(loc);
            
        }


    }

    private void DeclareColours(List<string> list)
    {
        var declarer = new ColourDeclarer(list, this);
        declarer.WriteDot(stringBuilder);
        declarer.WriteParts(stringBuilder);
        declarer.WriteJourneys(stringBuilder, journey);
    }
}

public class ColourDeclarer
{
    public List<string> Parts { get; }

    public ColourDeclarer(List<string> parts, TofmSystemParser tofmSystemParser)
    {
        Parts = parts;
    }

    public void WriteParts(StringBuilder stringBuilder)
    {
        stringBuilder.Append($@" <namedsort id=""Parts"" name=""Parts""> <cyclicenumeration>");
        foreach (var part in Parts)
        {
            stringBuilder.Append($@"<feconstan id=""{part}"" name=""Parts""/>");
        }
        stringBuilder.Append("</namedsort> </cyclicenumeration>");  
    }
    
    public void WriteDot(StringBuilder stringBuilder)
    {
        stringBuilder.Append($@" <namedsort id=""dot"" name=""dot"">
          <dot/></namedsort>");
    }

    public void WriteJourneys(StringBuilder builder, JourneyCollection journeys)
    {
        foreach (var partjour in journeys)
        {
            var part = partjour.Key;
            var journey = partjour.Value.Select(e => e.Name);
            foreach (var location in journey)
            {
                
            }
            

        }
        
        
    }
}

public static class LocationExtensions
{
    public static Location ToCapacityLocation(this Location location)
    {
        return new Location(location.Name+Hat, Infinity, )
        {
            IsProcessing = false
        };


    }
    public static readonly string DefaultCapacityColor = "dot";
    public static readonly string Hat = "_capacity";

    public static Invariant Infinity = new Invariant(ColourType.DefaultColorType.Name, 0, Infteger.PositiveInfinity);
}