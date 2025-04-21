using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using Fluentish.InjectableStatic.Generator.Extensions;
using System.Collections.Immutable;
using Fluentish.InjectableStatic.Generator.Models.Metadata;
using System.Linq;

namespace Fluentish.InjectableStatic.Generator.ValueProviders.Mappers
{
    internal static class MetadataMapper
    {
        public static AttributeModel[] ToAttributeModels(this ImmutableArray<AttributeData> attributes, out bool requireNullable)
        {
            requireNullable = false;
            var result = new AttributeModel[attributes.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = attributes[i].ToAttributeModel(out var attributeNullable);
                requireNullable |= attributeNullable;
            }
            return result;
        }

        public static GenericArgumentModel[] ToGenericArgumentModels(this IMethodSymbol methodSymbol, out bool requireNullable)
        {
            return methodSymbol.TypeArguments.ToGenericArgumentModels(methodSymbol.OriginalDefinition.TypeParameters, out requireNullable);
        }

        public static GenericArgumentModel[] ToGenericArgumentModels(this INamedTypeSymbol namedType, out bool requireNullable)
        {
            return namedType.TypeArguments.ToGenericArgumentModels(namedType.OriginalDefinition.TypeParameters, out requireNullable);
        }

        public static GenericArgumentModel[] ToGenericArgumentModels(this ImmutableArray<ITypeSymbol> typeArguments, ImmutableArray<ITypeParameterSymbol> typeParameters, out bool requireNullable)
        {
            var result = new GenericArgumentModel[typeArguments.Length];
            requireNullable = false;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = typeArguments[i].ToGenericArgumentModel(typeParameters[i], out var nullableArgument);
                requireNullable |= nullableArgument;
            }
            return result;
        }

        public static AttributeModel ToAttributeModel(this AttributeData attribute, out bool requireNullable)
        {
            requireNullable = false;
            var typeFullName = attribute.AttributeClass!.ToFullyQualifiedName(out var _);

            var arguments = new string[attribute.ConstructorArguments.Length];
            for (int i = 0; i < attribute.ConstructorArguments.Length; i++)
            {
                arguments[i] = attribute.ConstructorArguments[i].ToFullValue(out var argumentNullable);

                requireNullable |= argumentNullable;
            }

            var namedArguments = new (string name, string value)[attribute.NamedArguments.Length];
            for (int i = 0; i < attribute.NamedArguments.Length; i++)
            {
                var name = attribute.NamedArguments[i].Key;

                var value = attribute.NamedArguments[i].Value.ToFullValue(out var argumentNullable);
                requireNullable |= argumentNullable;

                namedArguments[i] = (name, value);
            }

            return new AttributeModel(
                typeFullName: typeFullName,
                arguments: arguments,
                namedArguments: namedArguments
            );
        }

        public static GenericArgumentModel ToGenericArgumentModel(this ITypeSymbol typeArgument, ITypeParameterSymbol typeParameter, out bool requireNullable)
        {
            requireNullable = false;

            if (
                !typeParameter.HasReferenceTypeConstraint
                && !typeParameter.HasValueTypeConstraint
                && !typeParameter.HasUnmanagedTypeConstraint
                && typeParameter.ConstraintTypes.Length == 0
                && !typeParameter.HasConstructorConstraint
                || typeArgument.Kind != SymbolKind.TypeParameter
            )
            {
                return new GenericArgumentModel(
                    name: typeArgument.Name,
                    constraints: null
                );
            }

            var typeConstraints = new string[typeParameter.ConstraintTypes.Length];
            for (int i = 0; i < typeParameter.ConstraintTypes.Length; i++)
            {
                typeConstraints[i] = typeParameter.ConstraintTypes[i].ToFullyQualifiedName(out var typeConstraintNullable);
                requireNullable |= typeConstraintNullable;
            }


            return new GenericArgumentModel(
                typeArgument.Name,
                new GenericArgumentConstraintsModel(
                    hasReferenceTypeConstraint: typeParameter.HasReferenceTypeConstraint,
                    hasValueTypeConstraint: typeParameter.HasValueTypeConstraint,
                    hasUnmanagedTypeConstraint: typeParameter.HasUnmanagedTypeConstraint,
                    hasTypeConstraint: typeParameter.ConstraintTypes.Length > 0,
                    typeConstraints: typeConstraints,
                    hasConstructorConstraint: typeParameter.HasConstructorConstraint
                )
            );
        }

        public static string ToFullyQualifiedName(this ITypeSymbol typeSymbol, out bool requireNullable)
        {
            var needsNullable = false;

            var returnType = new StringBuilder().AppendType(typeSymbol, ref needsNullable).ToString();
            requireNullable = needsNullable;

            return returnType;
        }
        public static string ToFullValue(this TypedConstant constant, out bool requireNullable)
        {
            requireNullable = false;
            if (constant.IsNull)
            {
                return "null";
            }

            if (constant.Kind == TypedConstantKind.Array)
            {
                var res = "{";
                for (int i = 0; i < constant.Values.Length; i++)
                {
                    if (i > 0)
                    {
                        res += ", ";
                    }
                    res += constant.Values[i].ToFullValue(out var itemNullable);
                    requireNullable |= itemNullable;
                }
                res += "}";
                return res;
            }

            if (constant.Kind == TypedConstantKind.Type || constant.Type!.SpecialType == SpecialType.System_Object)
            {
                var res = $"typeof({constant.Type!.ToFullyQualifiedName(out var typeNullable)})"; ;
                requireNullable |= typeNullable;
                return res;
            }

            if (constant.Kind == TypedConstantKind.Enum)
            {
                return "global::" + constant.ToCSharpString();
            }

            return constant.ToCSharpString();
        }

        public static ImmutableArray<AttributeData> WhereGeneratable(this ImmutableArray<AttributeData> attributes)
        {
            return attributes.Where(x =>
                    x.AttributeClass is not null
                    && x.AttributeClass.DeclaredAccessibility == Accessibility.Public
                )
                .Where(x =>
                {
                    var displayString = x.AttributeClass!.ToDisplayString();
                    return !displayString.Contains("System.Runtime.CompilerServices")
                        && displayString != "System.Diagnostics.ConditionalAttribute";
                })
                .ToImmutableArray();
        }
    }
}
