using Fluentish.InjectableStatic.Generator.Attributes;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    internal static class InjectableClassInfoProvider
    {
        public static IncrementalValuesProvider<InjectableClassInfo> GetInjectableClassInfoProvider(this IncrementalGeneratorInitializationContext context)
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
                            var targetTypeName = targetTypeArgument.Value!.ToString();

                            var injectableAttributeSymbol = compilation.GetTypeByMetadataName(targetTypeName);

                            if (injectableAttributeSymbol is null)
                            {
                                return InjectableClassInfo.Default;
                            }

                            var filterTypeArgument = attribute.NamedArguments.FirstOrDefault(x => x.Key == "FilterType");
                            var filterType = filterTypeArgument.Key is not null
                                ? (FilterType)filterTypeArgument.Value.Value!
                                : FilterType.Exclude;

                            var filteredTypesArgument = attribute.NamedArguments.FirstOrDefault(x => x.Key == "FilteredMembers");
                            string[] filteredTypes = filteredTypesArgument.Key is not null
                                ? filteredTypesArgument.Value.Values.Where(x => !x.IsNull).Select(x => x.Value!.ToString()).ToArray()
                                : [];

                            return new InjectableClassInfo(injectableAttributeSymbol, filterType, filteredTypes);
                        })
                        .Where(x => x != InjectableClassInfo.Default);

                    return selectedTypes;
                });
        }
    }
}
