namespace TmpmsChecker.Algorithm;

public interface IConfigurationGenerator
{
    public ReachableConfig[] GenerateConfigurations(Configuration configuration);
}