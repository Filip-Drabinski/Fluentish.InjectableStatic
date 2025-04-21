using Fluentish.InjectableStatic.Generator.Attributes;
using Fluentish.InjectableStatic.Generator.Extensions;
using Fluentish.InjectableStatic.Generator.GeneratedAttributes;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    internal static class InjectableStaticConfigurationProvider
    {
        public static IncrementalValueProvider<InjectableStaticConfigurationInfo> GetInjectableStaticConfigurationProvider(this IncrementalGeneratorInitializationContext context)
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
                        return new InjectableStaticConfigurationInfo(
                            endLine: newLineSymbol,
                            namespaceMode: NamespaceMode.Prefix,
                            @namespace: null
                        );
                    }
                    var targetTypeArgument = injectableStaticConfigurationAttribute.NamedArguments.FirstOrDefault(x=>x.Key == "Namespace");
                    var namespaceValue = targetTypeArgument.Key is not null 
                        ? targetTypeArgument.Value.Value as string
                    : null;

                    var namespaceModeArgument = injectableStaticConfigurationAttribute.NamedArguments.FirstOrDefault(x => x.Key == "NamespaceMode");
                    var namespaceMode = namespaceModeArgument.Key is not null && !namespaceModeArgument.Value.IsNull
                        ? (NamespaceMode)namespaceModeArgument.Value.Value!
                        : NamespaceMode.Prefix;

                    return new InjectableStaticConfigurationInfo(
                        endLine: newLineSymbol,
                        namespaceMode: namespaceMode,
                        @namespace: namespaceValue
                    );
                });
        }
    }
}
