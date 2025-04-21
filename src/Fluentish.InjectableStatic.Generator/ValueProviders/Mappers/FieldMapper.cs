using Fluentish.InjectableStatic.Generator.Models.Members;
using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Generator.ValueProviders.Mappers
{
    internal static class FieldMapper
    {
        public static bool TryParseFieldModel(this ISymbol symbol, TypeSerializer typeSerializer, out FieldModel fieldModel, out bool requireNullable)
        {
            if (symbol is not IFieldSymbol fieldSymbol)
            {
                requireNullable = false;
                fieldModel = default!;
                return false;
            }
            requireNullable = false;

            var attributeModels = fieldSymbol
                .GetAttributes()
                .WhereGeneratable()
                .ToAttributeModels(typeSerializer, out var attributeNullable);
            requireNullable |= attributeNullable;

            var type = typeSerializer.Serialize(fieldSymbol.Type, out var nullableType);
            requireNullable |= nullableType;

            var name = fieldSymbol.Name;
            var isMutable = !fieldSymbol.IsConst && !fieldSymbol.IsReadOnly;

            fieldModel = new FieldModel(
                attributes: attributeModels,
                type: type,
                name: name,
                isMutable: isMutable
            );
            return true;
        }
    }
}
