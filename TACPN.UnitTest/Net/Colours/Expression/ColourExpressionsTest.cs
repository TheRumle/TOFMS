using FluentAssertions;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;

namespace TACPN.UnitTest.Net.Colours.Expression;

public class ColourExpressionsTest
{
    public static readonly int N = 2;
    public static readonly int X = 5;
    private static readonly Colour P1 = new("P1");
    private static readonly Colour P2 = new("P2");
    private static readonly ColourType TestColour = new ColourType("Test", new[] { P1 });
    private static readonly Variable Variable = Variable.Create("VarP1", TestColour);



    [Fact]
    public void TextForTupleColour_ShouldBeAmount_Parenthesis_CommaSeparatedColor()
    {
        var colour = CreateTupleColour();
        var expression = new ColourExpression(colour, N);
        var varName = Variable.VariableNameFor(P1);
        expression.ExpressionText.Should().Be($"{N}'({varName}--, P1)");
    }



    [Fact]
    public void TextForColour_ShouldBeThatColour()
    {
        var expression = new ColourExpression(P1, TestColour, N);
        expression.ExpressionText.Should().Be($"{N}'{P1.Value}");
    }

    
    [Fact]
    public void TextForMultipleColourExpressions_ShouldBe_NOfFirst_Plus_XOfSecond()
    {
        ColourExpression[] subexpressions = {new ColourExpression(P1,TestColour, N),
            new ColourExpression(P2, TestColour, X)};
        
        var expression = new MultiColourExpression(subexpressions, TestColour);
        expression.ExpressionText.Should().Be($"{N}'{P1.Value} + {X}'{P2.Value}");
    }
    
    [Fact]
    public void TextForMultipleColourExpressions_WithTuple_ShouldBe_NOfFirst_Plus_XOfSecond()
    {
        var tuple = CreateTupleColour();
        ColourExpression[] subexpressions = {new ColourExpression(P1,TestColour, N),
            new ColourExpression(tuple, TestColour, X)};
        
        var expression = new MultiColourExpression(subexpressions, TestColour);
        expression.ExpressionText.Should().Be($"{N}'{P1.Value} + {X}'{tuple.Value}");
    }

    private static TupleColour CreateTupleColour()
    {
        IColourValue p1Decr = new VariableDecrement(Variable);
        var colour = new TupleColour(new[] { p1Decr, P1 }, TestColour);
        return colour;
    }
}

