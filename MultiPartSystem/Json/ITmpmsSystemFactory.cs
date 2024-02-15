namespace Tmpms.Json;

internal interface ITmpmsSystemFactory<T>
{
    TimedMultipartSystem Create(T structure);
}