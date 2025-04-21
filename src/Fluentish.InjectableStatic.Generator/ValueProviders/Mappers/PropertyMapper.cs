using Microsoft.CodeAnalysis;
using Fluentish.InjectableStatic.Generator.Models.Members;

namespace Fluentish.InjectableStatic.Generator.ValueProviders.Mappers
{
    internal static class PropertyMapper
    {
        public static bool TryParsePropertyModel(this ISymbol memberSymbol, out PropertyModel propertyModel, out bool requireNullable)
        {
            if (memberSymbol is not IPropertySymbol propertySymbol)
            {
                requireNullable = false;
                propertyModel = default!;
                return false;
            }
            requireNullable = false;

            var attributeModels = propertySymbol
                .GetAttributes()
                .WhereGeneratable()
                .ToAttributeModels(out var attributeNullable);
            requireNullable |= attributeNullable;

            var type = propertySymbol.Type.ToFullyQualifiedName(out var nullableType);
            requireNullable |= nullableType;

            var name = propertySymbol.Name;
            var isMutable = propertySymbol.SetMethod is not null && propertySymbol.SetMethod.DeclaredAccessibility == Accessibility.Public;
            var isReadable = propertySymbol.GetMethod is not null && propertySymbol.GetMethod.DeclaredAccessibility == Accessibility.Public;

            propertyModel = new PropertyModel(
                attributes: attributeModels,
                type: type,
                name: name,
                isReadable: isReadable,
                isMutable: isMutable
            );
            return true;
        }
    }
}
