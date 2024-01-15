using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

namespace Tmpms.Common.Json.Validators;

public static class FuncEnumerableExtensions
{
    public static Task<IEnumerable<InvalidJsonTofmException>>[] BeginValidationsOver<TFirstInput>(this 
        IEnumerable<Func<TFirstInput, IEnumerable<InvalidJsonTofmException>>> validationActions,TFirstInput input)
    {
        return validationActions.Select(e=> Task.Run(()=>e.Invoke(input))).ToArray();
    }
    
    public static Task<IEnumerable<InvalidJsonTofmException>>[] BeginValidationsOver<TFirstInput, TSecondInput>(this 
        IEnumerable<Func<TFirstInput, TSecondInput, IEnumerable<InvalidJsonTofmException>>> validationActions,TFirstInput inputOne, TSecondInput inputTwo)
    {
        return validationActions.Select(e=> Task.Run(()=>e.Invoke(inputOne, inputTwo))).ToArray();
    }
}