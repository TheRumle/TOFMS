using Common.Factories;
using Common.JsonTofms;
using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.JsonTofms.Models;
using Common.Move;
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
    private readonly MoveActionFactory _factory;

    public JsonTofmToDomainTofmParserTest(CentrifugeFixture centrifugeFixture)
    {
        systemText = centrifugeFixture.SystemWithOnlyComponentText;
        this._systemValidator = new TofmSystemValidator(
                new LocationValidator(new InvariantValidator()),
                new NamingValidator(),
                new MoveActionValidator()
            );
        
        _system = JsonConvert.DeserializeObject<TofmSystem>(this.systemText)!;
        _factory = new MoveActionFactory();
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
    
    
    [Fact]
    public async Task ShouldBeAbleToParseToDomainObjects()
    {
        JsonTofmToDomainTofmParser parser = new JsonTofmToDomainTofmParser(_systemValidator, _factory);
        IEnumerable<MoveAction> domains = await parser.ParseTofmsSystemJsonString(systemText);
        domains.Should().NotBeEmpty();
    } 
    
    
    
}