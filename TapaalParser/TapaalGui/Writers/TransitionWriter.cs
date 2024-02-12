using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;

namespace TapaalParser.TapaalGui.Writers;

public class TransitionWriter : TacpnUiXmlWriter<Transition>
{
    public TransitionWriter(Transition value) : base(value)
    {
    }

    public override void AppendAllText()
    {
        foreach (var place in Parseable.InvolvedPlaces)
            WritePlaces(place);

        WriteTransition(Parseable);


    }

    private void WriteTransition(Transition transition)
    {
        Append($@"<transition angle=""0"" displayName=""true"" id=""{transition.Name}"" infiniteServer=""false"" name=""{transition.Name}"" nameOffsetX=""0"" nameOffsetY=""0"" player=""0"" positionX=""50"" positionY=""50"" priority=""0"" urgent=""false""> ");
        WriteGuard(transition.Guard);
        Append(@"</transition>");
    }

    private void WriteGuard(ITransitionGuard transitionGuard)
    {
        if(transitionGuard == TransitionGuard.Empty) return;
        
        Append(@$"<condition>");
        Append($@"<text>");
        Append(transitionGuard.ToTapaalText());
        Append($@"</text>");
        AppendStructures(transitionGuard);
        Append(@$"</condition>");
    }

    private void AppendStructures(ITransitionGuard transitionGuard)
    {
        Append($@"<structure>");
        foreach (var orStatement in transitionGuard.Conditions)
        { 
            //Append("<and>");//TODO Fix and statements if necessary 
            CreateComparisonTagsString(orStatement.Comparisons); 
            //Append("</and>");
        }
        
        Append($@"</structure>");
    }

    private void CreateComparisonTagsString(IEnumerable<IColourComparison<ColourVariable, int>> orStatementComparisons)
    {
        if (orStatementComparisons.Count() > 1)
            Append("<or>");
        foreach (var comparison in orStatementComparisons)
        {
            var variableText = $@"<subterm> <variableref variable=""{comparison.Lhs.Name}""/> </subterm>";
            var (startTag, endTag) = GetTagFor(comparison.Operator);
            var colorValue = CreateColorValue(comparison);


            Append("<subterm>");
            Append(startTag);
            Append(variableText);
            Append(colorValue);
            Append(endTag);
            Append("</subterm>");
        }
        if (orStatementComparisons.Count() > 1)
            Append("</or>");
        
        
    }

    private string CreateColorValue(IColourComparison<ColourVariable, int> comparison)
    {
        if (comparison.Lhs.ColourType is IntegerRangedColour intRange)
        {
            return
                $@"<subterm> <finiteintrangeconstant value=""{comparison.Rhs}""> <finiteintrange end=""{intRange.MaxValue}""start=""0""/></finiteintrangeconstant></subterm>";
        }

        throw new NotImplementedException("Can only support color variables with color comparrison of Integer Colors.");
    }

    private (string startTag, string endTag) GetTagFor(ColourComparisonOperator comparisonOperator)
    {

        return (GetStartTag(comparisonOperator), GetEndTag(comparisonOperator));
    }

    private string GetEndTag(ColourComparisonOperator comparisonOperator)
    {
        switch (comparisonOperator)
        {
            case ColourComparisonOperator.Eq:
                return $"</equality>";
                break;
            case ColourComparisonOperator.Neq:
                throw new NotImplementedException();
            case ColourComparisonOperator.Leq:
                throw new NotImplementedException();
            case ColourComparisonOperator.Le:
                throw new NotImplementedException();
            case ColourComparisonOperator.Geq:
                throw new NotImplementedException();
            case ColourComparisonOperator.Gr:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(comparisonOperator), comparisonOperator, null);
        }
    }

    private string GetStartTag(ColourComparisonOperator comparisonOperator)
    {
        switch (comparisonOperator)
        {
            case ColourComparisonOperator.Eq:
                return $"<equality>"; 
                break;
            case ColourComparisonOperator.Neq:
                throw new NotImplementedException();
                break;
            case ColourComparisonOperator.Leq:
                throw new NotImplementedException();
                break;
            case ColourComparisonOperator.Le:
                throw new NotImplementedException();
                break;
            case ColourComparisonOperator.Geq:
                throw new NotImplementedException();
                break;
            case ColourComparisonOperator.Gr:
                throw new NotImplementedException();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(comparisonOperator), comparisonOperator, null);
        }
        
    }

    private void WritePlaces(Place location)
    {
        Append(
            $@"<place displayName=""true"" id=""{location.Name}"" initialMarking=""0"" invariant=""&lt; inf"" name=""{location.Name}"" nameOffsetX=""30"" nameOffsetY=""30"" positionX=""30"" positionY=""30"">
                      <type>
                        <text>{location.ColourType.Name}</text>
                        <structure>
                          <usersort declaration=""{location.ColourType.Name}""/>
                        </structure>
                      </type>");

        WriteInvariants(location);
        Append(@"</place>");
    }

    private void WriteInvariants(Place place)
    {
        InvariantWriter writer = new InvariantWriter(place.ColourInvariants);
        writer.AppendAllText();
        Append(writer.ToString());
    }
}