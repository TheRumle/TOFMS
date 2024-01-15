﻿using FluentAssertions;
using JsonFixtures.Tofms.Fixtures;
using Newtonsoft.Json;
using Tmpms.Common.Factories;
using Tmpms.Common.Json;
using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;
using Tmpms.Common.JsonTofms;
using Tmpms.Common.JsonTofms.ConsistencyCheck;
using Tmpms.Common.JsonTofms.ConsistencyCheck.Validators;

namespace Common.IntegrationTest.JsonTofms;

public class JsonTofmToDomainTofmParserTest : IClassFixture<CentrifugeFixture>
{
    private readonly MoveActionFactory _factory;
    private readonly TofmJsonSystem _jsonSystem;
    private readonly TofmSystemValidator _systemValidator;
    private readonly string systemText;

    public JsonTofmToDomainTofmParserTest(CentrifugeFixture centrifugeFixture)
    {
        systemText = centrifugeFixture.SystemWithOnlyComponentText;
        _systemValidator = new TofmSystemValidator(
            new LocationValidator(new InvariantValidator()),
            new NamingValidator(),
            new MoveActionValidator()
        );

        _jsonSystem = JsonConvert.DeserializeObject<TofmJsonSystem>(systemText)!;
        _factory = new MoveActionFactory();
    }

    [Fact]
    public void TheJsonParses()
    {
        var a = JsonConvert.DeserializeObject<TofmJsonSystem>(systemText);
        a.Should().NotBeNull();
        a!.Components.Should().NotBeEmpty();
        a.Parts.Should().NotBeEmpty();
    }

    [Fact]
    public void ShouldNotGiveValidationErrorsForCorrectSystem()
    {
        var errs = _systemValidator.Validate(_jsonSystem);
        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        invalidJsonTofmExceptions.Should()
            .BeEmpty(new ErrorFormatter(invalidJsonTofmExceptions.ToArray()).ToErrorString());
    }

    [Fact]
    public async Task ShouldBeAbleToParseToDomainObjects()
    {
        var parser = new JsonTofmToDomainTofmParser(_systemValidator, _factory);
        var domains = await parser.ParseTofmsSystemJsonString(systemText);
        domains.Journeys.Should().NotBeEmpty();
        domains.MoveActions.Should().NotBeEmpty();
        domains.Parts.Should().NotBeEmpty();
    }
}