namespace TmpmsChecker.Algorithm;

public interface IConfigurationGenerator
{
    public IEnumerable<ReachedState> GenerateConfigurations(Configuration configuration);
}