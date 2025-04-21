using Microsoft.CodeAnalysis;
using Fluentish.InjectableStatic.Generator.Models.Members;

namespace Fluentish.InjectableStatic.Generator.ValueProviders.Mappers
{
    internal static class EventMapper
    {
        public static bool TryParseEventModel(this ISymbol memberSymbol, out EventModel eventModel, out bool requireNullable)
        {
            if (memberSymbol is not IEventSymbol eventSymbol)
            {
                requireNullable = false;
                eventModel = default!;
                return false;
            }
            requireNullable = false;

            var attributeModels = eventSymbol
                .GetAttributes()
                .WhereGeneratable()
                .ToAttributeModels(out var attributeNullable);
            requireNullable |= attributeNullable;

            var type = eventSymbol.Type.ToFullyQualifiedName(out var nullableType);
            requireNullable |= nullableType;

            var name = eventSymbol.Name;
            var isAddable = eventSymbol.AddMethod is not null && eventSymbol.AddMethod.DeclaredAccessibility == Accessibility.Public;
            var isRemovable = eventSymbol.RemoveMethod is not null && eventSymbol.RemoveMethod.DeclaredAccessibility == Accessibility.Public;

            eventModel = new EventModel (
                attributes: attributeModels,
                type: type,
                name: name,
                isAddable: isAddable,
                isRemovable: isRemovable
            );
            return true;
        }
    }
}
