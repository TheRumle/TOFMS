using System.Text;
using Tofms.Common;
using Tofms.Common.Move;

namespace Xml;

public class SubnetDeclarer
{
    private readonly StringBuilder _stringBuilder;

    public SubnetDeclarer(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public void WriteComponent(MoveAction moveAction, IEnumerable<CapacityLocation> capacityLocations)
    {
        var capacitylizedNames = moveAction.InvolvedLocations.Select(e => e.ToCapacityLocation());
        IEnumerable<CapacityLocation> involvedCapacityLocations = capacityLocations.Where(e=>capacitylizedNames.Any(c => c==e));

        foreach (var capacityLocation in involvedCapacityLocations)
            WriteCapacityLocation(capacityLocation);

        foreach (var location in moveAction.InvolvedLocations)
            WriteLocation(location);

        
        TransitionWriter transitionWriter = new TransitionWriter(_stringBuilder, moveAction);
        transitionWriter.WriteTransition();
        
        
        
        
        _stringBuilder.Append($@"<net active=""true"" id=""{moveAction.Name}"" type=""P/T net"">");
    }

    private void WriteLocation(Location location)
    {
        _stringBuilder.Append($@"<place displayName=""true"" id=""{location.Name}"" initialMarking=""0"" invariant=""&lt; inf"" name=""{location.Name}"" nameOffsetX=""30"" nameOffsetY=""30"" positionX=""30"" positionY=""30"">
          <type>
            <text>{Colours.TokenColour}</text>
            <structure>
              <usersort declaration=""{Colours.TokenColour}""/>
            </structure>
          </type>
        </place>");
    }

    private void WriteCapacityLocation(CapacityLocation capacityLocation)
    {
        _stringBuilder.Append(
            $@"<place displayName=""true"" id=""{capacityLocation.Name}"" initialMarking=""{capacityLocation.Capacity}"" invariant=""&lt; inf"" name=""{capacityLocation.Name}"" nameOffsetX=""0"" nameOffsetY=""0"" positionX=""0"" positionY=""0"">
      <type>
        <text>dot</text>
        <structure>
          <usersort declaration=""{Colours.DefaultCapacityColor}""/>
        </structure>
      </type>
      <hlinitialMarking>
        <text>({capacityLocation.Capacity}'{Colours.DefaultCapacityColor})</text>
        <structure>
          <add>
            <subterm>
              <numberof>
                <subterm>
                  <numberconstant value=""{capacityLocation.Capacity}"">
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
    </place>");
    }
}

public class TransitionWriter
{
  public StringBuilder StringBuilder { get; }
  public MoveAction MoveAction { get; }

  public TransitionWriter(StringBuilder stringBuilder, MoveAction moveAction)
  {
    StringBuilder = stringBuilder;
    MoveAction = moveAction;
    
  }

  public void WriteTransition()
  {
  }
}