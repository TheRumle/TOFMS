namespace Common.JsonTofms.Models;

public record TofmSystem
{
    public List<String> Parts { get; set; }
    public List<TofmComponent> Components { get; set; }
}