using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Generator.Attributes
{
    internal static class InjectableNamespacePrefixAttributeSourceExtension
    {
        public static IncrementalGeneratorPostInitializationContext AddInjectableNamespacePrefixAttribute(this IncrementalGeneratorPostInitializationContext context)
        {
            context
                .AddSource(
                    "InjectableNamespacePrefixAttribute.g.cs",
                    """
                    namespace Fluentish.InjectableStatic
                    {

                        [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
                        public sealed class InjectableNamespacePrefixAttribute : System.Attribute
                        {
                            public string NamespacePrefix { get; }

                            public InjectableNamespacePrefixAttribute(string namespacePrefix)
                            {
                                NamespacePrefix = namespacePrefix;
                            }
                        }
                    }
                    """
                );
            return context;
        }
    }
}
