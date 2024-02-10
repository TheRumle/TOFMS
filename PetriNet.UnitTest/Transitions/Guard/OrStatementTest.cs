using FluentAssertions;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Transitions.Guard;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.UnitTest.Transitions.Guard;

public class OrStatementTest : VariableDependentTest
{
    
    [Fact]
    public void Should_Parse_SingleComparison_ToCorrectText()
    {
        var variable = CreateVariable("P1", 1);
        
        VariableComparison comparison = new VariableComparison(ColourComparisonOperator.Eq, variable, 0);
        var sut = OrStatement.FromPartComparisons([comparison], ColourType.FromValues(["P1"]));

        sut.ToTapaalText().Should().Be($"{variable.Name} EQ 0");
    }
    
    [Fact]
    public void Should_Parse_TwoComparisons_ToCorrectText()
    {

        var variable = CreateVariable("P1", 2);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variable, 0);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variable, 1);
        var sut = OrStatement.FromPartComparisons([comparison1,comparison2], ColourType.FromValues(["P1"]));


        sut.ToTapaalText().Should().Be($"({variable.Name} EQ 0 {OrStatement.SEPARATOR} {variable.Name} EQ 1)");
    }
    
    [Fact]
    public void Should_Parse_ThreeComparisons_ToCorrectText()
    {

        var variable = CreateVariable("P1", 3);
        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variable, 0);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variable, 1);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, variable, 2);
        var sut = OrStatement.FromPartComparisons([comparison1,comparison2, comparison3], ColourType.FromValues(["P1"]));


        sut.ToTapaalText().Should().Be($"(({variable.Name} EQ 0 {OrStatement.SEPARATOR} {variable.Name} EQ 1) {OrStatement.SEPARATOR} {variable.Name} EQ 2)");
    }
    
    
    [Fact]
    public void Should_Parse_TwoPartTypes_ToCorrectText()
    {

        var variableOne = CreateVariable("P1",1);
        var variableTwo = CreateVariable("P2",5);

        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variableOne, 0);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variableTwo, 1);
        var sut = OrStatement.FromPartComparisons([comparison1,comparison2], ColourType.FromValues(["P1", "P2"]));


        sut.ToTapaalText().Should().Be($"({variableOne.Name} EQ 0 {OrStatement.SEPARATOR} {variableTwo.Name} EQ 1)");
    }
    
    [Fact]
    public void Should_Parse_TwoPartTypes_MultipleComparisons_ToCorrectText()
    {

        var variableOne = CreateVariable("P1",5);
        var variableTwo = CreateVariable("P2",5);
        
        VariableComparison comparison1 = new VariableComparison(ColourComparisonOperator.Eq, variableOne, 0);
        VariableComparison comparison2 = new VariableComparison(ColourComparisonOperator.Eq, variableTwo, 1);
        VariableComparison comparison3 = new VariableComparison(ColourComparisonOperator.Eq, variableTwo, 2);
        var sut = OrStatement.FromPartComparisons([comparison1,comparison2, comparison3], ColourType.FromValues(["P1", "P2","P3"]));


        sut.ToTapaalText().Should().Be($"(({variableOne.Name} EQ 0 {OrStatement.SEPARATOR} {variableTwo.Name} EQ 1) {OrStatement.SEPARATOR} {variableTwo.Name} EQ 2)");
    }

    public OrStatementTest(MoveActionFixture fixture) : base(fixture)
    {
        
    }
}