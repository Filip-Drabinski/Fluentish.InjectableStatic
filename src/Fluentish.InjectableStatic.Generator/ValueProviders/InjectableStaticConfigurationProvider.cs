using Fluentish.InjectableStatic.Generator.Attributes;
using Fluentish.InjectableStatic.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    internal static class InjectableStaticConfigurationProvider
    {
        public static IncrementalValueProvider<InjectableStaticConfiguration> GetInjectableStaticConfigurationProvider(this IncrementalGeneratorInitializationContext context)
        {
            return context.CompilationProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Select((data, ct) =>
                {
                    var (compilation, optionsProvider) = data;

                    var newLineSymbol = optionsProvider.GlobalOptions.GetNewLineSymbol();

                    var attributes = compilation.Assembly.GetAttributes();

                    var injectableAttributeSymbol = compilation.GetInjectableStaticConfigurationAttribute();

                    var injectableStaticConfigurationAttribute = attributes
                        .FirstOrDefault(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, injectableAttributeSymbol));

                    if (injectableStaticConfigurationAttribute is null)
                    {
                        return new InjectableStaticConfiguration(
                            @namespace: null,
                            endLine: newLineSymbol
                        );
                    }

                    var targetTypeArgument = injectableStaticConfigurationAttribute.ConstructorArguments.First();
                    var namespacePrefixValue = targetTypeArgument.Value!.ToString();

                    return new InjectableStaticConfiguration(
                        namespacePrefixValue,
                        endLine: newLineSymbol
                    );
                });
        }
    }
}
