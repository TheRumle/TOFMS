using System.Runtime.CompilerServices;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public record MoveActionStructureValidationContext(IEnumerable<LocationDefinition> LocationStructures, IEnumerable<string> parts);

public class MoveActionValidator : IValidator<IEnumerable<MoveActionDefinition>,MoveActionStructureValidationContext>
{
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<MoveActionDefinition> locations, MoveActionStructureValidationContext moveActions)
    {
        var moveActionStructures = locations as MoveActionDefinition[] ?? locations.ToArray();
        var locationNameErrors = ValidateLocationNames(moveActionStructures, moveActions.LocationStructures);
        var partTypeErrors = ValidatePartTypes(moveActionStructures, moveActions.parts);
        locationNameErrors.AddRange(partTypeErrors);
        return locationNameErrors;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<MoveActionDefinition> values, MoveActionStructureValidationContext context)
    {
        return await Task.Run(() => Validate(values, context));
    }
    
    private static List<InvalidJsonTofmException> ValidateLocationNames(IEnumerable<MoveActionDefinition> values, IEnumerable<LocationDefinition> structures)
    {
        IEnumerable<string> locationNames = structures.Select(e => e.Name);
        List<InvalidJsonTofmException> errs = new List<InvalidJsonTofmException>();
        foreach (MoveActionDefinition structure in values)
        {
            var enumerable = locationNames as string[] ?? locationNames.ToArray();
            if (!enumerable.Contains(structure.From)) errs.Add(new UndefinedLocationException(structure, structure.From));
            if (!enumerable.Contains(structure.To)) errs.Add(new UndefinedLocationException(structure, structure.To));
        }

        return errs;
    }
    
    private static List<InvalidJsonTofmException> ValidatePartTypes(IEnumerable<MoveActionDefinition> values, IEnumerable<string> declaredPartTypes)
    {
        List<InvalidJsonTofmException> errs = new List<InvalidJsonTofmException>();
        var moveActionStructures = values as MoveActionDefinition[] ?? values.ToArray();
        var partTypes = declaredPartTypes as string[] ?? declaredPartTypes.ToArray();

        foreach (var move in moveActionStructures)
        {
            if(string.IsNullOrWhiteSpace(move.From)) AddEmptyLocationNameErr(errs, move.From);
            if(string.IsNullOrWhiteSpace(move.To)) AddEmptyLocationNameErr(errs, move.To);
            AppendPartErrors(move, errs, partTypes);
        }
        return errs;
    }

    private static void AppendPartErrors(MoveActionDefinition move, List<InvalidJsonTofmException> errs, string[] partTypes)
    {
        foreach (var value in move.Parts)
        {
            if (string.IsNullOrWhiteSpace(value.PartType))
            {
                AddEmptyPartTypeNameErr(errs, move.From);
                continue;
            }

            if (!partTypes.Contains(value.PartType))
                errs.Add(new UndefinedLocationException(move, value.PartType));
        }
    }

    public static void AddEmptyLocationNameErr<T>(List<InvalidJsonTofmException> err, T context)
    {
        err.Add(new LocationNameEmptyException<T>(context));
    }
    
    public static void AddEmptyPartTypeNameErr<T>(List<InvalidJsonTofmException> err, T context)
    {
        err.Add(new PartTypeNameEmptyException<T>(context));
    }
}