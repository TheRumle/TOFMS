using Tofms.Common.JsonTofms.ConsistencyCheck.Error;
using Tofms.Common.JsonTofms.Models;

namespace Tofms.Common.JsonTofms.ConsistencyCheck.Validators;

public class MoveActionValidator : IValidator<IEnumerable<MoveActionDefinition>, MoveActionStructureValidationContext>
{
    public List<InvalidJsonTofmException> _errs = new List<InvalidJsonTofmException>();
    
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<MoveActionDefinition> locations,
        MoveActionStructureValidationContext moveActions)
    {
        var moveActionStructures = locations as MoveActionDefinition[] ?? locations.ToArray();
        var locationNameErrors = ValidateLocationNames(moveActionStructures, moveActions.LocationStructures);
        var partTypeErrors = ValidatePartTypes(moveActionStructures, moveActions.parts);
        locationNameErrors.AddRange(partTypeErrors);
        return locationNameErrors;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<MoveActionDefinition> values,
        MoveActionStructureValidationContext context)
    {
        return await Task.Run(() => Validate(values, context));
    }

    private List<InvalidJsonTofmException> ValidateLocationNames(IEnumerable<MoveActionDefinition> values,
        IEnumerable<LocationDefinition> structures)
    {
        var locationNames = structures.Select(e => e.Name);
        foreach (var structure in values)
        {
            var locNames = locationNames as string[] ?? locationNames.ToArray();
            if (!locNames.Contains(structure.From))
                _errs.Add(new UndefinedLocationException(structure, structure.From));
            if (!locNames.Contains(structure.To)) 
                _errs.Add(new UndefinedLocationException(structure, structure.To));
        }

        return _errs;
    }

    private List<InvalidJsonTofmException> ValidatePartTypes(IEnumerable<MoveActionDefinition> values,
        IEnumerable<string> declaredPartTypes)
    {
        var _errs = new List<InvalidJsonTofmException>();
        var moveActionStructures = values as MoveActionDefinition[] ?? values.ToArray();
        var partTypes = declaredPartTypes as string[] ?? declaredPartTypes.ToArray();

        foreach (var move in moveActionStructures)
        {
            if (string.IsNullOrWhiteSpace(move.From)) AddEmptyLocationNameErr(move.From);
            if (string.IsNullOrWhiteSpace(move.To)) AddEmptyLocationNameErr(move.To);
            AppendPartErrors(move, partTypes);
        }

        return _errs;
    }

    private void AppendPartErrors(MoveActionDefinition move, string[] partTypes)
    {
        foreach (var value in move.Parts)
        {
            if (string.IsNullOrWhiteSpace(value.PartType))
                AddEmptyPartTypeNameErr(move.From);
        }
    }

    public void AddEmptyLocationNameErr<T>(T context)
    {
        _errs.Add(new LocationNameEmptyException<T>(context));
    }

    public void AddEmptyPartTypeNameErr<T>(T context)
    {
        _errs.Add(new PartTypeNameEmptyException<T>(context));
    }
}