using System.Text;
using Common;
using Tmpms;
using Tmpms.Journey;

namespace Xml;

internal class SharedPlaceDeclarationWriter
{
    public StringBuilder StringBuilder { get; }

    public SharedPlaceDeclarationWriter(StringBuilder stringBuilder)
    {
        StringBuilder = stringBuilder;
    }

    public void WritePlaces(IEnumerable<Location> locationDefinitions, IndexedJourneyCollection collection)
    {
        foreach (var location in locationDefinitions)
        {
            if (location.Name == Location.EndLocationName) WriteEndLocation(location, collection);
            else if (location.IsProcessing) WriteProcessingLocation(location, collection);
            else WriteBufferLocation(location);
        }
        
    }

    private void WriteEndLocation(Location location, IndexedJourneyCollection collection)
    {
      StringBuilder.Append($@"<shared-place initialMarking=""0"" invariant=""&lt; inf"" name=""{location.Name}"">");
      WriteInvariants(location, collection);
      StringBuilder.Append($@"   <type> <text>{Colours.TokenColour}</text> <structure><usersort declaration=""{Colours.TokenColour}""/> </structure> </type> </shared-place>");
    }

    private void WriteBufferLocation(Location location)
    {
        StringBuilder.Append($@"<shared-place initialMarking=""0"" invariant=""&lt; inf"" name=""{location.Name}"">
                                <type>
                                  <text>{Colours.TokenColour}</text>
                                  <structure>
                                    <usersort declaration=""{Colours.TokenColour}""/>
                                  </structure>
                                </type>
                              </shared-place>");
    }

    private void WriteProcessingLocation(Location location, IndexedJourneyCollection collection)
    {
        StringBuilder.Append($@"<shared-place initialMarking=""0"" invariant=""&lt; inf"" name=""{location.Name}"">");
        WriteInvariants(location, collection);
        StringBuilder.Append($@"   <type> <text>{Colours.TokenColour}</text> <structure><usersort declaration=""{Colours.TokenColour}""/> </structure> </type> </shared-place>");
    }

    public void WriteInvariants(Location location, IndexedJourneyCollection collection)
    {
      foreach (var inv in location.Invariants)
      {
        var part = inv.PartType;
        var v = collection[inv.PartType];
        foreach (var jourIndex in v)
        {
          int jourNumber = jourIndex.Key;

          WriteInvariant(inv, part, jourNumber);
        }
      }
    }

    public void WriteInvariant(Invariant inv, string part, int jourNumber)
    {
      var invMaxText = inv.Max == Infteger.PositiveInfinity ? "&lt; inf" : $"&lt;= {inv.Max}";
      StringBuilder.Append($@"<colorinvariant>
                                          <inscription inscription=""{invMaxText}""/>
                                          <colortype name=""{Colours.TokenColour}"">
                                            <color value=""{part}""/>
                                            <color value=""{jourNumber}""/>
                                          </colortype>
                                        </colorinvariant>");
    }

    public void WriteCapacityPlaces(IEnumerable<CapacityLocation> capacityLocations, IndexedJourneyCollection indexedJourneysCollection)
    {
      foreach (var capPlace in capacityLocations)
        StringBuilder.Append($@"<shared-place initialMarking=""{capPlace.Capacity}"" invariant=""&lt; inf"" name=""{capPlace.Name}"">
                                <type>
                                    <text>{Colours.DefaultCapacityColor}</text>
                                    <structure>
                                      <usersort declaration=""{Colours.DefaultCapacityColor}""/>
                                    </structure>
                                </type>
                                <hlinitialMarking>
                                  <text>({capPlace.Capacity}'dot)</text>
                                  <structure>
                                    <add>
                                      <subterm>
                                        <numberof>
                                          <subterm>
                                            <numberconstant value=""{capPlace.Capacity}"">
                                              <positive/>
                                            </numberconstant>
                                          </subterm>
                                          <subterm>
                                            <useroperator declaration=""{Colours.DefaultCapacityColor}""/>
                                          </subterm>
                                        </numberof>
                                      </subterm>
                                    </add>
                                  </structure>
                                </hlinitialMarking>
                              </shared-place>");
    }
}