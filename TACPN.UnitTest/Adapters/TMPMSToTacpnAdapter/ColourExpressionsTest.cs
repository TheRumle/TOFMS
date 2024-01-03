using FluentAssertions;
using TACPN.Net.Colours.Evaluatable;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.UnitTest.Adapters.TMPMSToTacpnAdapter;

public class ColourExpressionsTest
{
    public static readonly int Amount = 2;
    private static readonly Colour P1 = new("P1");
    private static readonly ColourType TestColour = new ColourType("Test", new[] { P1 });
    private static readonly Variable Variable = Variable.Create("VarP1", TestColour);



    [Fact]
    public void TextForCombinedColour_ShouldBeAmount_Parenthesis_CommaseparatedColor()
    {
        IColourValue p1Decr = new VariableDecrement(Variable);

        var colour = new TupleColour(new[] { p1Decr, P1 }, TestColour);
        var expression = new ColourExpression(colour, Amount);
        var varName = Variable.VariableNameFor(P1);
        expression.ExpressionText.Should().Be($"2'({varName}--, P1)");
    }
    
}

