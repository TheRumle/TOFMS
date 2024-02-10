using System.Collections;
using System.Text;
using JsonFixtures;
using TACPN.Places;
using TapaalParser.TapaalGui.Writers;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Xml;

namespace TaapalParser.UnitTests.DeclarationWriters;

public class LocationWriterTest : WriterTest
{
    public LocationGenerator LocationGenerator;
    public LocationWriterTest(MoveActionFixture fixture) : base(fixture)
    {
        LocationGenerator = fixture.LocationGenerator;
    }
    
    public string WriteOriginalLocations(IEnumerable<Location> locations, IEnumerable<CapacityLocation> capacityLocations, IndexedJourneyCollection indexed)
    {
        SharedPlaceDeclarationWriter declarationWriter = new SharedPlaceDeclarationWriter(new StringBuilder());
        declarationWriter.WritePlaces(locations, indexed);
        declarationWriter.WriteCapacityPlaces( capacityLocations, indexed);
        return RemoveWhiteSpace(declarationWriter.StringBuilder.ToString());
    }


    public CreateWriter()
    {
        TacpnUiXmlWriter<(IEnumerable<Place>, IEnumerable<CapacityPlace>)> writer = new LocationWriter();
    }


    public void ShouldWriteSame()
    {
        
        
    }
}