using Tofms.Common.JsonTofms.ConsistencyCheck.Error;
using Tofms.Common.JsonTofms.Models;

namespace Tofms.Common.JsonTofms.ConsistencyCheck.Validators;

public class MissingDefinitionValidator : IValidator<TofmComponent>
{
    private List<InvalidJsonTofmException> _errors = new();

    public void VerifyMoves(List<MoveActionDefinition> moveActionDefinitions, string parentName)
    {
        foreach (MoveActionDefinition moveActionDefinition in moveActionDefinitions)
        {
            var componentName = $"{parentName}.{moveActionDefinition.Name}";
             
            if(moveActionDefinition.Name is null) 
               AddError(parentName ,nameof( moveActionDefinition.Name));
            
            if(moveActionDefinition.From is null) 
               AddError(parentName, nameof(moveActionDefinition.From));
            
            if(moveActionDefinition.To is null) 
               AddError(parentName, nameof( moveActionDefinition.To));
            
            if(moveActionDefinition.Parts is null) 
               AddError(componentName, nameof( moveActionDefinition.Parts));
            else 
            {
               VerifyParts(moveActionDefinition.Parts, nameof(moveActionDefinition.Parts), componentName); 
            }
           
           if(moveActionDefinition.EmptyAfter is null) 
               AddError(componentName, nameof(moveActionDefinition.EmptyAfter));
           else
           {
               VerifyEmpty(moveActionDefinition.EmptyAfter,moveActionDefinition);
           }
           
           if(moveActionDefinition.EmptyBefore is null) 
               AddError(componentName, nameof(moveActionDefinition.EmptyBefore));
           else
           {
               VerifyEmpty(moveActionDefinition.EmptyBefore,moveActionDefinition);
           }
        }
    }

    private void VerifyEmpty(List<string> emptyAfter, MoveActionDefinition parent)
    {
        if (!emptyAfter.Any())
        {
            AddError(parent.Name, nameof(parent.EmptyAfter));
        }
    }

    private void VerifyParts(List<PartConsumptionDefinition> parts, string parentName, string fieldname)
    {
      
        foreach (var partConsumptionDefinition in parts)
        {
            if(partConsumptionDefinition.PartType is null)
                AddError(parentName, fieldname);
        }
    }

    public void AddError(string parent, string fieldName)
    {
        _errors.Add(new MissingDefinitionError(parent, fieldName));
    }
    
    public IEnumerable<InvalidJsonTofmException> Validate(TofmComponent component)
    {
        VerifyMoves(component.Moves, component.Name);
        VerifyLocations(component.Locations);
        return _errors;
    }

    private void VerifyLocations(List<LocationDefinition> componentLocations)
    {
        foreach (var locationDefinition in componentLocations)
        {
            var name = locationDefinition.Name;
               if (locationDefinition.Name is null)
                   AddError(name, nameof(locationDefinition.Name));
               if (locationDefinition.Capacity is 0)
                   AddError(name, nameof(locationDefinition.Capacity));
               
               if (locationDefinition.Invariants is null)
                   AddError(name, nameof(locationDefinition.Invariants));
               else
               {
                   VerifyInvariants(locationDefinition.Invariants, locationDefinition.Name);
               }

        }
    }

    private void VerifyInvariants(List<InvariantDefinition> locationDefinitionInvariants, string parentName)
    {
        foreach (var locationDefinitionInvariant in locationDefinitionInvariants)
        {
            if(locationDefinitionInvariant.Part is null)
                AddError(parentName, nameof(locationDefinitionInvariant.Part));
        }
    }

    public Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(TofmComponent values)
    {
        return Task.Run(() => Validate(values));
    }
}