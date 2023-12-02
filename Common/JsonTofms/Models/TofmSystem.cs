namespace Tofms.Common.JsonTofms.Models;

public record TofmSystem
{
    public List<string>? Parts { get; set; }
    public List<TofmComponent>? Components { get; set; }

    public const string PRODUCTNAME = "Parts";
}