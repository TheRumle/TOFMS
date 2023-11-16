using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public interface INamingValidator : IValidator<IEnumerable<LocationStructure>, IEnumerable<MoveActionStructure>>
{
    
}
public class NamingValidator : INamingValidator
{
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<LocationStructure> locations, IEnumerable<MoveActionStructure> moveActions)
    {

        bool allThreeNamesAreSame = false;
        List<InvalidJsonTofmException> errs = new List<InvalidJsonTofmException>();
        foreach (LocationStructure location in locations)
            foreach (var action in moveActions)
            {
                foreach ((int Amount, string PartType) value in action.Parts)
                    allThreeNamesAreSame = CompareNames(value, action, location, errs, allThreeNamesAreSame);
                
                if (!allThreeNamesAreSame && location.Name == action.Name)
                    AddError(errs, location, action, location.Name);
            }

        return errs;
    }

    private static bool CompareNames((int Amount, string PartType) value, MoveActionStructure action,
        LocationStructure location, List<InvalidJsonTofmException> errs, bool foundThree)
    {
        if (value.PartType == action.Name && value.PartType == location.Name)
        {
            AddError(errs, location, action, value, location.Name);
            foundThree = true;
            return foundThree;
        }

        if (value.PartType == action.Name)
        {
            AddError(errs, value, action, action.Name);
            return foundThree;
        }

        if (value.PartType == location.Name)
            AddError(errs, value, location, location.Name);
        return foundThree;
    }

    private static void AddError<TFirst, TSecond, TThird>(List<InvalidJsonTofmException> errs, TFirst f, TSecond s, TThird t, string name)
    {
        errs.Add(new DuplicateNameException<TFirst,TSecond,TThird>(f,s,t,name));   
    }
    
    private static void AddError<TFirst, TSecond>(List<InvalidJsonTofmException> errs, TFirst f, TSecond s, string name)
    {
        errs.Add(new DuplicateNameException<TFirst,TSecond>(f,s,name));   
    }

    public Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<LocationStructure> values, IEnumerable<MoveActionStructure> context)
    {
        return Task.Run(() => Validate(values, context));
    }
}