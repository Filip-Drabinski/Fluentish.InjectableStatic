using Fluentish.InjectableStatic.Generator.GeneratedAttributes;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    internal static class InjectableClassInfoProvider
    {
        public static IncrementalValuesProvider<InjectableStaticInfo> GetInjectableClassInfoProvider(this IncrementalGeneratorInitializationContext context)
        {
            return context.CompilationProvider
                .SelectMany((compilation, ct) =>
                {
                    var attributes = compilation.Assembly.GetAttributes();

                    var injectableAttributeSymbol = compilation.GetInjectableAttribute();

                    var matchingAttributes = attributes
                        .Where(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, injectableAttributeSymbol));

                    var selectedTypes = matchingAttributes
                        .Select(attribute =>
                        {
                            var targetTypeArgument = attribute.ConstructorArguments[0];
                            var targetType = (ITypeSymbol)targetTypeArgument.Value!;
                            var fullMetadataName = targetType.ContainingNamespace + "." + targetType.MetadataName;
                            var injectableAttributeSymbol = compilation.GetTypeByMetadataName(fullMetadataName);

                            if (injectableAttributeSymbol is null)
                            {
                                return InjectableStaticInfo.Default;
                            }

                            var filterTypeArgument = attribute.NamedArguments.FirstOrDefault(x => x.Key == "FilterType");
                            var filterType = filterTypeArgument.Key is not null
                                ? (FilterType)filterTypeArgument.Value.Value!
                                : FilterType.Exclude;

                            var filteredTypesArgument = attribute.NamedArguments.FirstOrDefault(x => x.Key == "FilteredMembers");
                            string[] filteredTypes = filteredTypesArgument.Key is not null
                                ? filteredTypesArgument.Value.Values.Where(x => !x.IsNull).Select(x => x.Value!.ToString()).ToArray()
                                : [];

                            return new InjectableStaticInfo(injectableAttributeSymbol, filterType, filteredTypes);
                        })
                        .Where(x => x != InjectableStaticInfo.Default);

                    return selectedTypes;
                });
        }
    }
}
