using FluentAssertions;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.UnitTest.Transitions.Guard;

public class TransitionGuardTest : VariableDependentTest
{

    [Fact]
    public void SeparateTwoOrs_With_And()
    {
        var firstVar = CreateVariable("P1", 3);
        var secondVar = CreateVariable("P2", 11);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, firstVar, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, firstVar, 2);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, secondVar, 10);
        VariableComparison comparison4 = new VariableComparison(ColourComparisonOperator.Eq, secondVar, 1);

        var ct = ColourType.FromValues(["P1", "P2"]);
        var firstOrs = OrStatement.FromPartComparisons([comparison1,comparison2], ct);
        var secondOrs = OrStatement.FromPartComparisons([comparison3,comparison4],ct);

        TransitionGuard guard = TransitionGuard.FromAndedOrs([firstOrs, secondOrs]);
        
        guard.ToTapaalText().Should().Be($"({firstOrs.ToTapaalText()} {TransitionGuard.GUARDSEPARATOR} {secondOrs.ToTapaalText()})");
    }
    
    [Fact]
    public void SeparateThreeOrs_WithParenthesis()
    {
        var firstVar = CreateVariable("P1", 3);
        var secondVar = CreateVariable("P2", 11);
        var thirdVar = CreateVariable("P3", 4);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, firstVar, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, firstVar, 2);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, secondVar, 10);
        VariableComparison comparison4 = new VariableComparison(ColourComparisonOperator.Eq, thirdVar, 1);
        
        var ct = ColourType.FromValues(["P1", "P2","P3"]);

        var firstOrs = OrStatement.FromPartComparisons([comparison1,comparison2],ct);
        var secondOrs = OrStatement.FromPartComparisons([comparison3],ct);
        var thirdOrs = OrStatement.FromPartComparisons([comparison4],ct);

        TransitionGuard guard = TransitionGuard.FromAndedOrs([firstOrs, secondOrs,thirdOrs]);
        
        guard.ToTapaalText().Should().Be($"({firstOrs.ToTapaalText()} {TransitionGuard.GUARDSEPARATOR} {secondOrs}) {TransitionGuard.GUARDSEPARATOR} {thirdOrs})");
    }
    
    [Fact]
    public void SeparateFourOrs_WithParenthesis()
    {
        var firstVar = CreateVariable("P1", 11);
        var secondVar = CreateVariable("P2", 11);
        var thirdVar = CreateVariable("P3", 30);
        var fourVar = CreateVariable("P4", 10);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, firstVar, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, secondVar, 2);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, thirdVar, 3);
        VariableComparison comparison4 = new VariableComparison(ColourComparisonOperator.Eq, fourVar, 4);
        var ct = ColourType.FromValues(["P1", "P2", "P4", "P5"]);

        var firstOrs = OrStatement.FromPartComparisons([comparison1],ct);
        var secondOrs = OrStatement.FromPartComparisons([comparison2],ct);
        var thirdOrs= OrStatement.FromPartComparisons([comparison3],ct);
        var fourthOrs = OrStatement.FromPartComparisons([comparison4],ct);
        TransitionGuard guard = TransitionGuard.FromAndedOrs([firstOrs, secondOrs,thirdOrs, fourthOrs]);
        
        guard.ToTapaalText().Should().Be($"({firstOrs.ToTapaalText()} {TransitionGuard.GUARDSEPARATOR} {secondOrs}) {TransitionGuard.GUARDSEPARATOR} ({thirdOrs} {TransitionGuard.GUARDSEPARATOR} {fourthOrs})");
    }
    
    [Fact]
    public void SeparateFiveOrs_WithParenthesis()
    {
        var firstVar = CreateVariable("P1", 10);
        var secondVar = CreateVariable("P2", 10);
        var thirdVar = CreateVariable("P3", 30);
        var fourVar = CreateVariable("P4", 10);
        var fiveVar = CreateVariable("P5", 15);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, firstVar, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, secondVar, 2);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, thirdVar, 3);
        VariableComparison comparison4 = new VariableComparison(ColourComparisonOperator.Eq, fourVar, 4);
        VariableComparison comparison5 = new VariableComparison(ColourComparisonOperator.Eq, fiveVar, 5);
        var ct = ColourType.FromValues(["P1", "P2"]);

        var firstOrs = OrStatement.FromPartComparisons([comparison1],ct);
        var secondOrs = OrStatement.FromPartComparisons([comparison2],ct);
        var thirdOrs= OrStatement.FromPartComparisons([comparison3],ct);
        var fourthOrs = OrStatement.FromPartComparisons([comparison4],ct);
        var fiveOrs = OrStatement.FromPartComparisons([comparison5],ct);
        TransitionGuard guard = TransitionGuard.FromAndedOrs([firstOrs, secondOrs,thirdOrs, fourthOrs, fiveOrs]);

        var a = guard.ToTapaalText();
        
        guard.ToTapaalText().Should().Be($"({firstOrs.ToTapaalText()} {TransitionGuard.GUARDSEPARATOR} {secondOrs}) {TransitionGuard.GUARDSEPARATOR} ({thirdOrs} {TransitionGuard.GUARDSEPARATOR} {fourthOrs}) {TransitionGuard.GUARDSEPARATOR} {fiveOrs}");
    }
    
    
    
    [Fact]
    public void OnlyOneAnd_IsSameAsOr()
    {
        var firstVar = CreateVariable("P1", 2);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, firstVar, 1);
        var ct = ColourType.FromValues(["P1"]);

        var firstOrs = OrStatement.FromPartComparisons([comparison1], ct);

        TransitionGuard guard = TransitionGuard.FromAndedOrs([firstOrs]);
        guard.ToTapaalText().Should().Be($"{firstOrs.ToTapaalText()}");
    }

    public TransitionGuardTest(MoveActionFixture fixture) : base(fixture)
    {
    }
}