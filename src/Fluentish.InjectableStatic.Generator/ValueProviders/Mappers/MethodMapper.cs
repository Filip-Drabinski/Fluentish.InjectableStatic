using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Fluentish.InjectableStatic.Generator.Models.Members;

namespace Fluentish.InjectableStatic.Generator.ValueProviders.Mappers
{
    internal static class MethodMapper
    {
        public static bool TryParseMethodModel(this ISymbol memberSymbol, out MethodModel methodModel, out bool requireNullable)
        {
            if (
                memberSymbol is not IMethodSymbol methodSymbol
                || methodSymbol.MethodKind == MethodKind.PropertySet
                || methodSymbol.MethodKind == MethodKind.PropertyGet
                || methodSymbol.MethodKind == MethodKind.EventAdd
                || methodSymbol.MethodKind == MethodKind.EventRemove
                || methodSymbol.MethodKind == MethodKind.EventRaise
                || methodSymbol.MethodKind == MethodKind.BuiltinOperator
                || methodSymbol.MethodKind == MethodKind.UserDefinedOperator
                || methodSymbol.MethodKind == MethodKind.Conversion
            )
            {
                methodModel = default!;
                requireNullable = false;
                return false;
            }
            requireNullable = false;

            var attributeModels = methodSymbol
                .GetAttributes()
                .WhereGeneratable()
                .ToAttributeModels(out var attributeNullable);
            requireNullable |= attributeNullable;

            var returnType = methodSymbol.ReturnType.ToFullyQualifiedName(out var returnTypeNullable);
            requireNullable |= returnTypeNullable;
            
            var parameterModels = methodSymbol.Parameters.ToMethodParameterModels(out var parametersNullable);
            requireNullable |= parametersNullable;
            
            var genericArguments = methodSymbol.ToGenericArgumentModels(out var genericTypesNullable);
            requireNullable |= genericTypesNullable;

            methodModel = new MethodModel(
                attributes: attributeModels,
                returnType: returnType,
                name: methodSymbol.Name,
                genericArguments: genericArguments,
                parameters: parameterModels
            );
            return true;
        }
        public static MethodParameterModel[] ToMethodParameterModels(this ImmutableArray<IParameterSymbol> parameters, out bool requireNullable)
        {
            requireNullable = false;
            var result = new MethodParameterModel[parameters.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = parameters[i].ToMethodParameterModel(out var parameterNullable);
                requireNullable |= parameterNullable;
            }
            return result;
        }
        public static MethodParameterModel ToMethodParameterModel(this IParameterSymbol parameter, out bool requireNullable)
        {
            requireNullable = parameter.NullableAnnotation == NullableAnnotation.Annotated;

            var typeName = parameter.Type.ToFullyQualifiedName(out var nullableType);
            requireNullable |= nullableType;

            var parameterName = parameter.Name;

            var attributes = parameter.GetAttributes().WhereGeneratable().ToAttributeModels(out var attributeNullable);
            requireNullable |= attributeNullable;

            var modifier = parameter switch
            {
                { IsParams: true } => "params ",
                { RefKind: RefKind.In } => "in ",
                { RefKind: RefKind.Out } => "out ",
                { RefKind: RefKind.Ref } => "ref ",
                _ => string.Empty
            };

            return new MethodParameterModel(
                attributes: attributes,
                modifier: modifier,
                type: typeName,
                name: parameterName
            );
        }

    }
}
