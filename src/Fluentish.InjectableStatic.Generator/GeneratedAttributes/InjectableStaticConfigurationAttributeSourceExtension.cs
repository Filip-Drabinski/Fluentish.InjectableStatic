using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Generator.GeneratedAttributes
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
                        public enum NamespaceMode
                        {
                            Prefix = 0,
                            Const = 1,
                        }

                        [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
                        public sealed class InjectableStaticConfigurationAttribute : System.Attribute
                        {
                            public NamespaceMode NamespaceMode { get; set; }
                            public string Namespace { get; set; }
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
