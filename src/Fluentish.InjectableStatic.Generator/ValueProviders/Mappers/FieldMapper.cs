using Microsoft.CodeAnalysis;
using Fluentish.InjectableStatic.Generator.Models.Members;

namespace Fluentish.InjectableStatic.Generator.ValueProviders.Mappers
{
    internal static class FieldMapper
    {
        public static bool TryParseFieldModel(this ISymbol symbol, out FieldModel fieldModel, out bool requireNullable)
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
                .ToAttributeModels(out var attributeNullable);
            requireNullable |= attributeNullable;

            var type = fieldSymbol.Type.ToFullyQualifiedName(out var nullableType);
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
