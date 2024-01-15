namespace Tmpms.Common.Json;

internal interface ITmpmsSystemFactory<T>
{
    TimedMultipartSystem Create(T structure);
}