namespace TmpmsChecker.Algorithm;

public interface IConfigurationExplorer
{
    public ReachableConfig[] GenerateConfigurations(Configuration configuration);
}