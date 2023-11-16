using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public record MoveActionStructureValidationContext(IEnumerable<LocationStructure> LocationStructures, IEnumerable<string> parts);

public class MoveActionValidator : IValidator<IEnumerable<MoveActionStructure>,MoveActionStructureValidationContext>
{
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<MoveActionStructure> values, MoveActionStructureValidationContext context)
    {
        var moveActionStructures = values as MoveActionStructure[] ?? values.ToArray();
        var locationNameErrors = ValidateLocationNames(moveActionStructures, context.LocationStructures);
        var partTypeErrors = ValidatePartTypes(moveActionStructures, context.parts);
        locationNameErrors.AddRange(partTypeErrors);
        return locationNameErrors;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<MoveActionStructure> values, MoveActionStructureValidationContext context)
    {
        return await Task.Run(() => Validate(values, context));
    }
    
    private static List<InvalidJsonTofmException> ValidateLocationNames(IEnumerable<MoveActionStructure> values, IEnumerable<LocationStructure> structures)
    {
        IEnumerable<string> locationNames = structures.Select(e => e.Name);
        List<InvalidJsonTofmException> errs = new List<InvalidJsonTofmException>();
        foreach (MoveActionStructure structure in values)
        {
            var enumerable = locationNames as string[] ?? locationNames.ToArray();
            if (!enumerable.Contains(structure.From)) errs.Add(new UndefinedLocationException(structure, structure.From));
            if (!enumerable.Contains(structure.To)) errs.Add(new UndefinedLocationException(structure, structure.To));
        }

        return errs;
    }
    
    private static List<InvalidJsonTofmException> ValidatePartTypes(IEnumerable<MoveActionStructure> values, IEnumerable<string> declaredPartTypes)
    {
        List<InvalidJsonTofmException> errs = new List<InvalidJsonTofmException>();
        var moveActionStructures = values as MoveActionStructure[] ?? values.ToArray();
        var partTypesInMoves = moveActionStructures.SelectMany(e => e.Parts).Select(e => e.PartType);
        var partTypes = declaredPartTypes as string[] ?? declaredPartTypes.ToArray();
        
        foreach (var move in moveActionStructures)
            foreach (var (amount, partType) in move.Parts) 
                if (!partTypes.Contains(partType)) errs.Add(new UndefinedLocationException(move, partType));

        return errs;
    }
}