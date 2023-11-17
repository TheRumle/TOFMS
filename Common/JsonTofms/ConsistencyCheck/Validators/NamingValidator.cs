using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public class NamingValidator : INamingValidator
{
    readonly List<InvalidJsonTofmException>  _errs = new List<InvalidJsonTofmException>();

    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<LocationDefinition> locations,
        IEnumerable<MoveActionDefinition> moveActions)
    {
        var allThreeNamesAreSame = false;
        foreach (var location in locations)
        {
            if (!location.Name.IsAlphaNumericOnly())
                AddAlphaNumericViolation(location, location.Name);
                
                
            foreach (var action in moveActions)
            {
                if (!action.Name.IsAlphaNumericOnly()) AddAlphaNumericViolation(action, action.Name);

                foreach (var value in action.Parts)
                    allThreeNamesAreSame = CompareNames(value, action, location, allThreeNamesAreSame);
                

                if (!allThreeNamesAreSame && location.Name == action.Name)
                    AddDuplicateNameError(location, action, location.Name);
            }
        }

        return _errs;
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