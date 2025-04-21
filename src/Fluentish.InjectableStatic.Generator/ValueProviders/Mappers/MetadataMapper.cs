using Fluentish.InjectableStatic.Generator.Models.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;

namespace Fluentish.InjectableStatic.Generator.ValueProviders.Mappers
{
    public class TypeSerializer
    {
        private readonly ConcurrentDictionary<ITypeSymbol, (string value, bool requireNullable)> _cache;
        public TypeSerializer()
        {
            _cache = new ConcurrentDictionary<ITypeSymbol, (string value, bool requireNullable)>(SymbolEqualityComparer.IncludeNullability);
        }
        public string Serialize(ITypeSymbol typeSymbol, out bool requireNullable)
        {
            var needsNullable = false;

            var returnType = ToFullyQualifiedType(typeSymbol, ref needsNullable).ToString();
            requireNullable = needsNullable;

            return returnType;
        }

        private string ToFullyQualifiedType(ITypeSymbol type, ref bool requireNullable)
        {
            if (_cache.TryGetValue(type, out var cacheHit))
            {
                requireNullable |= cacheHit.requireNullable;
                return cacheHit.value;
            }
            var isNullable = false;
            string result;
            switch (type)
            {
                case IArrayTypeSymbol arrayType:
                    result = ToFullyQualifiedType(arrayType.ElementType, ref isNullable) + "[]";
                    break;
                case IPointerTypeSymbol pointerType:
                    result = ToFullyQualifiedType(pointerType.PointedAtType, ref isNullable) + "*";
                    break;
                case INamedTypeSymbol { IsTupleType: true } tupleType:
                    result = ToFullyQualifiedTuple(tupleType, ref isNullable);
                    break;
                case INamedTypeSymbol { IsTupleType: false } namedType:
                    result = ToFullyQualifiedNamedType(namedType, ref isNullable);
                    break;
                case IDynamicTypeSymbol:
                    result = "dynamic";
                    break;
                case IFunctionPointerTypeSymbol funcPtr:
                    var funcDisplayString = funcPtr.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                    result = funcDisplayString;
                    break;
                default:
                    var displayString = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    result = displayString;
                    break;
            }

            _cache.TryAdd(type, (result, isNullable));

            requireNullable |= isNullable || type.NullableAnnotation == NullableAnnotation.Annotated;
            return result;
        }

        private string ToFullyQualifiedTuple(INamedTypeSymbol tupleType, ref bool requireNullable)
        {
            var elements = tupleType.TupleElements;

            var result = "(";
            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                result += ToFullyQualifiedType(element.Type, ref requireNullable);
                if (!string.IsNullOrEmpty(element.Name))
                {
                    result += " " + element.Name;
                }

                if (i < elements.Length - 1)
                {
                    result += ", ";
                }
            }
            result += ")";
            return result;
        }

        private string ToFullyQualifiedNamedType(INamedTypeSymbol typeSymbol, ref bool requireNullable)
        {
            if (
                typeSymbol.SpecialType != SpecialType.None
                && typeSymbol.SpecialType != SpecialType.System_DateTime
                && typeSymbol.TypeKind != TypeKind.Interface
            )
            {
                return typeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            }
            var result = "";

            if (typeSymbol.ContainingType != null)
            {
                result += ToFullyQualifiedNamedType(typeSymbol.ContainingType, ref requireNullable) + ".";
            }
            else if (
                typeSymbol.ContainingNamespace != null
                && !typeSymbol.ContainingNamespace.IsGlobalNamespace
            )
            {
                result += "global::" + typeSymbol.ContainingNamespace.ToDisplayString() + ".";
            }


            result += typeSymbol.Name;

            if (typeSymbol.TypeArguments.Length > 0)
            {
                result += "<";
                for (int i = 0; i < typeSymbol.TypeArguments.Length; i++)
                {
                    result += ToFullyQualifiedType(typeSymbol.TypeArguments[i], ref requireNullable);

                    if (i < typeSymbol.TypeArguments.Length - 1)
                    {
                        result += ", ";
                    }
                }
                result += ">";
            }

            if (typeSymbol.Name != "Nullable" && typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
            {
                result += "?";
            }
            return result;
        }

    }

    internal static class MetadataMapper
    {
        public static AttributeModel[] ToAttributeModels(this ImmutableArray<AttributeData> attributes, TypeSerializer typeSerializer, out bool requireNullable)
        {
            requireNullable = false;
            var result = new AttributeModel[attributes.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = attributes[i].ToAttributeModel(typeSerializer, out var attributeNullable);
                requireNullable |= attributeNullable;
            }
            return result;
        }

        public static GenericArgumentModel[] ToGenericArgumentModels(this IMethodSymbol methodSymbol, TypeSerializer typeSerializer, out bool requireNullable)
        {
            return methodSymbol.TypeArguments.ToGenericArgumentModels(methodSymbol.OriginalDefinition.TypeParameters, typeSerializer, out requireNullable);
        }

        public static GenericArgumentModel[] ToGenericArgumentModels(this INamedTypeSymbol namedType, TypeSerializer typeSerializer, out bool requireNullable)
        {
            return namedType.TypeArguments.ToGenericArgumentModels(namedType.OriginalDefinition.TypeParameters, typeSerializer, out requireNullable);
        }

        public static GenericArgumentModel[] ToGenericArgumentModels(this ImmutableArray<ITypeSymbol> typeArguments, ImmutableArray<ITypeParameterSymbol> typeParameters, TypeSerializer typeSerializer, out bool requireNullable)
        {
            var result = new GenericArgumentModel[typeArguments.Length];
            requireNullable = false;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = typeArguments[i].ToGenericArgumentModel(typeSerializer, typeParameters[i], out var nullableArgument);
                requireNullable |= nullableArgument;
            }
            return result;
        }

        public static AttributeModel ToAttributeModel(this AttributeData attribute, TypeSerializer typeSerializer, out bool requireNullable)
        {
            requireNullable = false;
            var typeFullName = typeSerializer.Serialize(attribute.AttributeClass!, out var attributeNullable);
            requireNullable |= attributeNullable;

            var arguments = new string[attribute.ConstructorArguments.Length];
            for (int i = 0; i < attribute.ConstructorArguments.Length; i++)
            {
                arguments[i] = attribute.ConstructorArguments[i].ToFullValue(typeSerializer, out var argumentNullable);

                requireNullable |= argumentNullable;
            }

            var namedArguments = new (string name, string value)[attribute.NamedArguments.Length];
            for (int i = 0; i < attribute.NamedArguments.Length; i++)
            {
                var name = attribute.NamedArguments[i].Key;

                var value = attribute.NamedArguments[i].Value.ToFullValue(typeSerializer, out var argumentNullable);
                requireNullable |= argumentNullable;

                namedArguments[i] = (name, value);
            }

            return new AttributeModel(
                typeFullName: typeFullName,
                arguments: arguments,
                namedArguments: namedArguments
            );
        }

        public static GenericArgumentModel ToGenericArgumentModel(this ITypeSymbol typeArgument, TypeSerializer typeSerializer, ITypeParameterSymbol typeParameter, out bool requireNullable)
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
                typeConstraints[i] = typeSerializer.Serialize(typeParameter.ConstraintTypes[i], out var typeConstraintNullable);
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

        public static string ToFullValue(this TypedConstant constant, TypeSerializer typeSerializer, out bool requireNullable)
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
                    res += constant.Values[i].ToFullValue(typeSerializer, out var itemNullable);
                    requireNullable |= itemNullable;
                }
                res += "}";
                return res;
            }

            if (constant.Kind == TypedConstantKind.Type || constant.Type!.SpecialType == SpecialType.System_Object)
            {
                var res = $"typeof({typeSerializer.Serialize(constant.Type!, out var typeNullable)})"; ;
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
