using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Test.Snapshot.Sources
{
    public static class GeneratedSourceResultExtensions
    {
        public static bool IsPostInitializationOutput(this GeneratedSourceResult sourceResult)
        {
            return sourceResult.HintName == "InjectableStaticConfigurationAttribute.g.cs"
                || sourceResult.HintName == "InjectableStaticAttribute.g.cs";
        }
    }
}