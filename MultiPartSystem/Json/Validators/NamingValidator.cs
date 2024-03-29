﻿using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Validators;

internal class NamingValidator : JsonValidator<IEnumerable<LocationDefinition>, IEnumerable<MoveActionDefinition>>
{
    readonly List<InvalidJsonTmpmsException> _errs = new();

    public IEnumerable<InvalidJsonTmpmsException> PerformValidation(IEnumerable<LocationDefinition> locations,
        IEnumerable<MoveActionDefinition> moveActions)
    {
        var definitions = locations as LocationDefinition[] ?? locations.ToArray();
        var locationDefinitions = locations as LocationDefinition[] ?? definitions.ToArray();
        var moveActionDefinitions = moveActions as MoveActionDefinition[] ?? moveActions.ToArray();

        foreach (var location in locationDefinitions)
        {
            if (!location.Name.IsAlphaNumericOnly())
                AddAlphaNumericViolation(location, location.Name);
        }

        foreach (var action in moveActionDefinitions)
        {
            ValidatePartNames(action);
            ValidateShouldBeEmptySets(action);
        }

        ValidateNoDuplicateNames(locationDefinitions, moveActionDefinitions);

        return _errs;
    }

    public override Task<IEnumerable<InvalidJsonTmpmsException>>[] ValidationTasksFor(IEnumerable<LocationDefinition> inputs, IEnumerable<MoveActionDefinition> context)
    {
        return new[]
        {
            Task.Run(() => PerformValidation(inputs, context))
        };
    }


    private void ValidateNoDuplicateNames(IEnumerable<LocationDefinition> locations,
        IEnumerable<MoveActionDefinition> moveActions)
    {
        var allThreeNamesAreSame = false;

        foreach (var location in locations)
        {
            var moveActionDefinitions = moveActions as MoveActionDefinition[] ?? moveActions.ToArray();
            foreach (var action in moveActionDefinitions)
            {
                if (!action.Name.IsAlphaNumericOnly()) AddAlphaNumericViolation(action, action.Name);

                foreach (var value in action.Parts)
                    allThreeNamesAreSame = CompareNames(value.Key, action, location, allThreeNamesAreSame);


                if (!allThreeNamesAreSame && location.Name == action.Name)
                    AddDuplicateNameError(location, action, location.Name);
            }
        }
    }

    private void ValidatePartNames(MoveActionDefinition action)
    {
        foreach (var partConsumptionDefinition in action.Parts)
            if (!partConsumptionDefinition.Key.IsAlphaNumericOnly())
                AddAlphaNumericViolation(action, partConsumptionDefinition.Key);
    }

    private void ValidateShouldBeEmptySets(MoveActionDefinition action)
    {
        foreach (var s in action.EmptyBefore)
            if (!s.IsAlphaNumericOnly())
                AddAlphaNumericViolation(action, s);

        foreach (var s in action.EmptyAfter)
            if (!s.IsAlphaNumericOnly())
                AddAlphaNumericViolation(action, s);
    }

    private bool CompareNames(string partType, MoveActionDefinition action,
        LocationDefinition location, bool foundThree)
    {

        if (partType == action.Name && partType == location.Name)
        {
            AddDuplicateNameError(location, action, partType, location.Name);
            foundThree = true;
            return foundThree;
        }

        if (partType == action.Name)
        {
            AddDuplicateNameError(partType, action, action.Name);
            return foundThree;
        }

        if (partType == location.Name)
            AddDuplicateNameError(partType, location, location.Name);
        return foundThree;
    }

    private void AddDuplicateNameError<TFirst, TSecond, TThird>(TFirst f, TSecond s,
        TThird t, string name)
    {
        _errs.Add(new DuplicateNameException<TFirst, TSecond, TThird>(f, s, t, name));
    }

    private void AddDuplicateNameError<TFirst, TSecond>(TFirst f, TSecond s, string name)
    {
        _errs.Add(new DuplicateNameException<TFirst, TSecond>(f, s, name));
    }

    private void AddAlphaNumericViolation<TFirst>(TFirst value, string violation)
    {
        _errs.Add(new AlphaNumericViolation<TFirst>(value, violation));
    }
    
    
}