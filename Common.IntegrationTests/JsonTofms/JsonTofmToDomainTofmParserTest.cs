﻿using FluentAssertions;
using JsonFixtures.Tofms.Fixtures;
using Newtonsoft.Json;
using Tofms.Common.Factories;
using Tofms.Common.JsonTofms;
using Tofms.Common.JsonTofms.ConsistencyCheck;
using Tofms.Common.JsonTofms.ConsistencyCheck.Error;
using Tofms.Common.JsonTofms.ConsistencyCheck.Validators;
using Tofms.Common.JsonTofms.Models;

namespace Common.IntegrationTest.JsonTofms;

public class JsonTofmToDomainTofmParserTest : IClassFixture<CentrifugeFixture>
{
    private readonly MoveActionFactory _factory;
    private readonly TofmSystem _system;
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

        _system = JsonConvert.DeserializeObject<TofmSystem>(systemText)!;
        _factory = new MoveActionFactory();
    }

    [Fact]
    public void TheJsonParses()
    {
        var a = JsonConvert.DeserializeObject<TofmSystem>(systemText);
        a.Should().NotBeNull();
        a!.Components.Should().NotBeEmpty();
        a.Parts.Should().NotBeEmpty();
    }

    [Fact]
    public void ShouldNotGiveValidationErrorsForCorrectSystem()
    {
        var errs = _systemValidator.Validate(_system);
        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        invalidJsonTofmExceptions.Should()
            .BeEmpty(new ErrorFormatter(invalidJsonTofmExceptions.ToArray()).ToErrorString());
    }

    [Fact]
    public async Task ShouldBeAbleToParseToDomainObjects()
    {
        var parser = new JsonTofmToDomainTofmParser(_systemValidator, _factory);
        var domains = await parser.ParseTofmsSystemJsonString(systemText);
        domains.Should().NotBeEmpty();
    }
}