using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Validators;

public class NamingValidator : INamingValidator
{
    readonly List<InvalidJsonTofmException>  _errs = new();

    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<LocationDefinition> locations,
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



    private void ValidateNoDuplicateNames(IEnumerable<LocationDefinition> locations, IEnumerable<MoveActionDefinition> moveActions)
    {
        var allThreeNamesAreSame = false;

        foreach (var location in locations)
        {
            var moveActionDefinitions = moveActions as MoveActionDefinition[] ?? moveActions.ToArray();
            foreach (var action in moveActionDefinitions)
            {
                if (!action.Name.IsAlphaNumericOnly()) AddAlphaNumericViolation(action, action.Name);

                foreach (var value in action.Parts)
                    allThreeNamesAreSame = CompareNames(value, action, location, allThreeNamesAreSame);


                if (!allThreeNamesAreSame && location.Name == action.Name)
                    AddDuplicateNameError(location, action, location.Name);
            }
        }
    }

    private void ValidatePartNames(MoveActionDefinition action)
    {
        foreach (var partConsumptionDefinition in action.Parts)
            if (!partConsumptionDefinition.PartType.IsAlphaNumericOnly()) 
                AddAlphaNumericViolation(action, partConsumptionDefinition.PartType);
    }

    private void ValidateShouldBeEmptySets(MoveActionDefinition action)
    {
        foreach (var s in action.EmptyBefore)
            if (!s.IsAlphaNumericOnly()) AddAlphaNumericViolation(action,s);
        
        foreach (var s in action.EmptyAfter)
            if (!s.IsAlphaNumericOnly()) 
                AddAlphaNumericViolation(action,s);
    }


    public Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<LocationDefinition> values,
        IEnumerable<MoveActionDefinition> context)
    {
        return Task.Run(() => Validate(values, context));
    }

    private bool CompareNames(PartConsumptionDefinition value, MoveActionDefinition action,
        LocationDefinition location, bool foundThree)
    {
        
        if (value.PartType == action.Name && value.PartType == location.Name)
        {
            AddDuplicateNameError(location, action, value, location.Name);
            foundThree = true;
            return foundThree;
        }

        if (value.PartType == action.Name)
        {
            AddDuplicateNameError(value, action, action.Name);
            return foundThree;
        }

        if (value.PartType == location.Name)
            AddDuplicateNameError(value, location, location.Name);
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