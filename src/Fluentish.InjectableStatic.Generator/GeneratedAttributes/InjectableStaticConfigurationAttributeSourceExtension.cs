using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Generator.Attributes
{
    internal static class InjectableStaticConfigurationAttributeSourceExtension
    {
        public static IncrementalGeneratorPostInitializationContext AddInjectableStaticConfigurationAttribute(this IncrementalGeneratorPostInitializationContext context)
        {
            context
                .AddSource(
                    "InjectableStaticConfigurationAttribute.g.cs",
                    """
                    namespace Fluentish.InjectableStatic
                    {

                        [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
                        public sealed class InjectableStaticConfigurationAttribute : System.Attribute
                        {
                            public string NamespacePrefix { get; }
                    
                            public InjectableStaticConfigurationAttribute(string namespacePrefix)
                            {
                                NamespacePrefix = namespacePrefix;
                            }
                        }
                    }
                    """
                );
            return context;
        }

        public static INamedTypeSymbol? GetInjectableStaticConfigurationAttribute(this Compilation compilation)
        {
            return compilation.GetTypeByMetadataName("Fluentish.InjectableStatic.InjectableStaticConfigurationAttribute");
        }
    }
}
