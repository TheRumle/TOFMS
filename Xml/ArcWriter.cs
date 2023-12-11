using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using Tofms.Common;
using Tofms.Common.Move;

namespace Xml;

public class ArcWriter
{
    private readonly string transition;
    private readonly JourneyCollection _journeys;
    public StringBuilder StringBuilder { get; }
    private MoveAction _moveAction { get; }

    public ArcWriter(StringBuilder stringBuilder, MoveAction moveAction, JourneyCollection collection)
    {
        StringBuilder = stringBuilder;
        _moveAction = moveAction;
        this.transition = moveAction.Name;
        this._journeys = collection;
    }



    public void WriteArcs()
    {
        var from = _moveAction.From;
        AddInhibitorArc(_moveAction.EmptyBefore);
        HandleFrom(_moveAction.From);
        HandleTo(_moveAction.To);
    }

    private void HandleTo(Location to)
    {
      var amount = _moveAction.PartsToMove.Select(e => e.Value).Sum();
      StringBuilder.Append($@"<arc id=""{_moveAction.Name}To{to.Name}"" inscription=""1"" nameOffsetX=""41"" nameOffsetY=""9"" source=""{_moveAction.Name}"" target=""{to.Name}"" type=""normal"" weight=""1"">");
      WriteHlWithJourneyInscription( _moveAction.PartsToMove);
      StringBuilder.Append("</arc>");

      ConsumeFromCapPlace(to.ToCapacityLocation(), to.Capacity - amount);
    }

    private void WriteHlWithJourneyInscription(HashSet<KeyValuePair<string, int>> partsToMove)
    {
      var minusText = _moveAction.From.IsProcessing ? "--" : ""; 
      var closeTag = _moveAction.From.IsProcessing ? "  </predecessor> </subterm>" : ""; 
      var startTag = _moveAction.From.IsProcessing ? "<subterm> <predecessor>" : ""; 
      
      
      StringBuilder.Append($@"<hlinscription> <text>(");
      var first = partsToMove.First();
      StringBuilder.Append($@"{first.Value}'({first.Key},{Colours.VariableNameForPart(first.Key)}{minusText})");
      foreach (var other in partsToMove.Skip(1))
      {
        StringBuilder.Append($@" + {other.Value}'({other.Key},{Colours.VariableNameForPart(other.Key)}{minusText})");
      }
      StringBuilder.Append($@")</text>");
      
      foreach (var partToMove in partsToMove) 
      {

        StringBuilder.Append($@"<structure>
                                  <numberof>
                                    <subterm>
                                      <numberconstant value=""{partToMove.Value}"">
                                        <positive/>
                                      </numberconstant>
                                    </subterm>
                                    <subterm>
                                      <tuple>
                                        <subterm>
                                          <useroperator declaration=""{partToMove.Key}""/>
                                        </subterm>
                                          {startTag}
                                        <subterm>
                                          <variable refvariable=""{Colours.VariableIdForPart(partToMove.Key)}""/>
                                        </subterm>
                                          {closeTag}
                                      </tuple>
                                    </subterm>
                                  </numberof>
                                </structure>");
      }
      StringBuilder.Append($@"</hlinscription>");
    }

    private void HandleFrom(Location from)
    {
      if (_moveAction.EmptyAfter.Contains(from)) WriteCapacityPlaceEmptyAfterContainsFrom();
      else HandleToNotInEmptyAfter();
      
      
      AddInhibitorArc(_moveAction.EmptyAfter.Where(e=>e.Name != from.Name));
      
      
      StringBuilder.Append($@"<arc id=""{from.Name}To{transition}"" inscription=""[0,inf)"" nameOffsetX=""41"" nameOffsetY=""9"" source=""{from.Name}"" target=""{_moveAction.Name}"" type=""timed"" weight=""1"">");
      foreach (var amountOfPart in _moveAction.PartsToMove)
      {
          WriteTokenColourInterval(amountOfPart.Key);
      }
      WriteHlWithJourneyIncInscription( _moveAction.PartsToMove, from);
      StringBuilder.Append("</arc>");
    }

    private void WriteHlWithJourneyIncInscription(HashSet<KeyValuePair<string, int>> partsToMove, Location location)
    {


      
      StringBuilder.Append($@"<hlinscription> <text>(");
      var first = partsToMove.First();
      StringBuilder.Append($@"{first.Value}'({first.Key},{Colours.VariableNameForPart(first.Key)})");
      foreach (var other in partsToMove.Skip(1))
      {
        StringBuilder.Append($@" + {other.Value}'({other.Key},{Colours.VariableNameForPart(other.Key)})");
      }
      StringBuilder.Append($@")</text>");
      
      foreach (var partToMove in partsToMove)
      {

        StringBuilder.Append($@"<structure>
                                  <numberof>
                                    <subterm>
                                      <numberconstant value=""{partToMove.Value}"">
                                        <positive/>
                                      </numberconstant>
                                    </subterm>
                                    <subterm>
                                      <tuple>
                                        <subterm>
                                          <useroperator declaration=""{partToMove.Key}""/>
                                        </subterm>
                                        <subterm>
                                          <variable refvariable=""{Colours.VariableIdForPart(partToMove.Key)}""/>
                                        </subterm>
                                      </tuple>
                                    </subterm>
                                  </numberof>
                                </structure>");
      }
      StringBuilder.Append($@"</hlinscription>");
    }

    private void WriteTokenColourInterval(string partType)
    {
      var journeyIndexes = _journeys[partType].Select(e=>e.Key);
      var guard = _moveAction.From.Invariants.First(e => e.PartType == partType);
      foreach (var index in journeyIndexes)
      {
        StringBuilder.Append($@"<colorinterval>
                                  <inscription inscription=""{guard.ToInvariantText()}""/>
                                  <colortype name=""{Colours.TokenColour}"">
                                    <color value=""{partType}""/>
                                    <color value=""{index}""/>
                                  </colortype>
                                </colorinterval>");
        
      }

      
    }

    private void HandleToNotInEmptyAfter()
    {
      var to = _moveAction.To;
      var toHat = to.ToCapacityLocation();
      var amount = _moveAction.PartsToMove.Count;
      
      ProduceToCapPlace(toHat, amount);
    }

    private void ProduceToCapPlace(CapacityLocation toHat, int amount)
    {
      //Write into capacity place
      StringBuilder.Append(
        $@"<arc id=""{_moveAction.Name}To{toHat.Name}"" inscription=""1"" nameOffsetX=""41"" nameOffsetY=""9"" source=""{_moveAction.Name}"" target=""{toHat.Name}"" type=""normal"" weight=""1"">
      <hlinscription>
        <text>{amount}'{Colours.DefaultCapacityColor}</text>
        <structure>
          <numberof>
            <subterm>
              <numberconstant value=""{amount}"">
                <positive/>
              </numberconstant>
            </subterm>
            <subterm>
              <useroperator declaration=""{Colours.DefaultCapacityColor}""/>
            </subterm>
          </numberof>
        </structure>
      </hlinscription>
    </arc>");
    }

    private void WriteCapacityPlaceEmptyAfterContainsFrom()
    {
      var from = _moveAction.From;
      var toHat = from.ToCapacityLocation();
      
      //Write into capacity place
      ProduceToCapPlace(toHat, from.Capacity);
      var toConsume = _moveAction.PartsToMove.Select(e => e.Value).Sum();
      
      //Consume from capacity place
      var amount = from.Capacity - toConsume;
      ConsumeFromCapPlace(toHat, amount);
    }

    private void ConsumeFromCapPlace(CapacityLocation toHat, int toConsume)
    {
      StringBuilder.Append(
        $@"<arc id=""{toHat.Name}To{_moveAction.Name}"" inscription=""[0,inf)"" nameOffsetX=""0"" nameOffsetY=""-22"" source=""{toHat.Name}"" target=""{_moveAction.Name}"" type=""timed"" weight=""1"">
                <hlinscription>
                  <text>{toConsume}'{Colours.DefaultCapacityColor}</text>
                  <structure>
                    <numberof>
                      <subterm>
                        <numberconstant value=""{toConsume}"">
                          <positive/>
                        </numberconstant>
                      </subterm>
                      <subterm>
                        <useroperator declaration=""{Colours.DefaultCapacityColor}""/>
                      </subterm>
                    </numberof>
                  </structure>
                </hlinscription>
              </arc>");
    }

    private void AddInhibitorArc(IEnumerable<Location> locations)
    {
        foreach (var location in locations)
        {
          StringBuilder.Append(
            $@"<arc id=""{location.Name}To{_moveAction.Name}I"" inscription=""[0,inf)"" nameOffsetX=""0"" nameOffsetY=""0"" source=""{location.Name}"" target=""{transition}"" type=""tapnInhibitor"" weight=""1"">
                    <hlinscription>
                      <text>1'{Colours.DefaultCapacityColor}.all</text>
                      <structure>
                        <numberof>
                          <subterm>
                            <numberconstant value=""1"">
                              <positive/>
                            </numberconstant>
                          </subterm>
                          <subterm>
                            <all>
                              <usersort declaration=""{Colours.DefaultCapacityColor}""/>
                            </all>
                          </subterm>
                        </numberof>
                      </structure>
                    </hlinscription>
                  </arc>");
        }
    }
}