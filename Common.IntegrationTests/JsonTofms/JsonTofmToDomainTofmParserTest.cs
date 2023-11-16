using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.JsonTofms.Models;
using FluentAssertions;
using JsonFixtures.Fixtures;
using Newtonsoft.Json;
using Xunit;

namespace Common.IntegrationTest.JsonTofms;

public class JsonTofmToDomainTofmParserTest : IClassFixture<CentrifugeFixture>
{
    private readonly string systemText;
    private readonly TofmSystemValidator _systemValidator;
    private readonly TofmSystem _system;

    public JsonTofmToDomainTofmParserTest(CentrifugeFixture centrifugeFixture)
    {
        systemText = centrifugeFixture.SystemWithOnlyComponentText;
        this._systemValidator = new TofmSystemValidator(
                new LocationValidator(new InvariantValidator()),
                new NamingValidator(),
                new MoveActionValidator()
            );
        
        this._system = JsonConvert.DeserializeObject<TofmSystem>(this.systemText)!;
    }

    [Fact]
    public void TheJsonParses()
    {
        var a = JsonConvert.DeserializeObject<TofmSystem>(this.systemText);
        a.Should().NotBeNull();
        a!.Components.Should().NotBeEmpty();
        a.Parts.Should().NotBeEmpty();
    }

    [Fact]
    public void ShouldNotGiveValidationErrorsForCorrectSystem()
    {
        IEnumerable<InvalidJsonTofmException> errs = _systemValidator.Validate(this._system);
        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        invalidJsonTofmExceptions.Should().BeEmpty(new ErrorFormatter(invalidJsonTofmExceptions.ToArray()).ToErrorString());
    } 
    
    
    
}