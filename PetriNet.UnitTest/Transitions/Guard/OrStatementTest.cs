using FluentAssertions;
using JsonFixtures;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.UnitTest.Transitions.Guard;

public class OrStatementTest : VariableDependentTest
{
    
    [Fact]
    public void Should_Parse_SingleComparison_ToCorrectText()
    {
        var variable = CreateVariable("P1", 1);
        
        VariableComparison comparison = new VariableComparison(ColourComparisonOperator.Eq, variable, 1);
        var sut = OrStatement.WithConditions([comparison]);

        sut.ToTapaalText().Should().Be($"{variable.Name} EQ 1");
    }
    
    [Fact]
    public void Should_Parse_TwoComparisons_ToCorrectText()
    {

        var variable = CreateVariable("P1", 2);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variable, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variable, 2);
        var sut = OrStatement.WithConditions([comparison1,comparison2]);


        sut.ToTapaalText().Should().Be($"({variable.Name} EQ 1 {OrStatement.SEPARATOR} {variable.Name} EQ 2)");
    }
    
    [Fact]
    public void Should_Parse_ThreeComparisons_ToCorrectText()
    {

        var variable = CreateVariable("P1", 3);
        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variable, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variable, 2);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, variable, 3);
        var sut = OrStatement.WithConditions([comparison1,comparison2, comparison3]);


        sut.ToTapaalText().Should().Be($"(({variable.Name} EQ 1 {OrStatement.SEPARATOR} {variable.Name} EQ 2) {OrStatement.SEPARATOR} {variable.Name} EQ 3)");
    }
    
    
    [Fact]
    public void Should_Parse_TwoPartTypes_ToCorrectText()
    {

        var variableOne = CreateVariable("P1",1);
        var variableTwo = CreateVariable("P2",5);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variableOne, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variableTwo, 2);
        var sut = OrStatement.WithConditions([comparison1,comparison2]);


        sut.ToTapaalText().Should().Be($"({variableOne.Name} EQ 1 {OrStatement.SEPARATOR} {variableTwo.Name} EQ 2)");
    }
    
    [Fact]
    public void Should_Parse_TwoPartTypes_MultipleComparisons_ToCorrectText()
    {

        var variableOne = CreateVariable("P1",5);
        var variableTwo = CreateVariable("P2",5);
        
        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variableOne, 1);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variableTwo, 2);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, variableTwo, 3);
        var sut = OrStatement.WithConditions([comparison1,comparison2, comparison3]);


        sut.ToTapaalText().Should().Be($"(({variableOne.Name} EQ 1 {OrStatement.SEPARATOR} {variableTwo.Name} EQ 2) {OrStatement.SEPARATOR} {variableTwo.Name} EQ 3)");
    }

    public OrStatementTest(MoveActionFixture fixture) : base(fixture)
    {
        
    }
}