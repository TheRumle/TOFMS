using Tmpms.Common.Json.Errors;

namespace Tmpms.Common.Json.Validators;

public static class FuncEnumerableExtensions
{
    public static Task<IEnumerable<InvalidJsonTmpmsException>>[] BeginValidationsOver<TFirstInput>(this 
        IEnumerable<Func<TFirstInput, IEnumerable<InvalidJsonTmpmsException>>> validationActions,TFirstInput input)
    {
        return validationActions.Select(e=> Task.Run(()=>e.Invoke(input))).ToArray();
    }
}