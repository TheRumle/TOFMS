using JsonFixtures;
using TACPN.Colours.Values;

namespace TACPN.UnitTest.Transitions.Guard;

public class VariableDependentTest  : IClassFixture<MoveActionFixture>
{
    public Func<string, int, ColourVariable> CreateVariable { get; set; }
    public VariableDependentTest(MoveActionFixture fixture)
    {
        this.CreateVariable = MoveActionFixture.VariableForPart;
    }
}