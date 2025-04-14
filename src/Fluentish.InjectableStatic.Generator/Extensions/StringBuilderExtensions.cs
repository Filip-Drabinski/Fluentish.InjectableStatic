using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Immutable;
using System.Text;

namespace Fluentish.InjectableStatic.Generator.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string value)
        {
            if (!condition)
            {
                return builder;
            }

            builder.Append(value);

            return builder;
        }

        public static StringBuilder AppendIndentation(this StringBuilder builder, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                builder.Append("    ");
            }

            return builder;
        }

        public static StringBuilder AppendReferenceKind(this StringBuilder builder, RefKind refKind)
        {
            if (refKind == RefKind.In)
            {
                builder.Append("in ");
            }
            else if (refKind == RefKind.Out)
            {
                builder.Append("out ");
            }
            else if (refKind == RefKind.Ref)
            {
                builder.Append("ref ");
            }
            return builder;
        }

        public static StringBuilder AppendAccessibility(this StringBuilder builder, Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                    builder.Append("private ");
                    break;
                case Accessibility.Protected:
                    builder.Append("protected ");
                    break;
                case Accessibility.Internal:
                    builder.Append("internal ");
                    break;
                case Accessibility.Public:
                    builder.Append("public ");
                    break;
                default:
                    break;
            }
            return builder;
        }

        public static StringBuilder AppendTypeArguments(this StringBuilder builder, ImmutableArray<ITypeSymbol> typeArguments)
        {
            if (typeArguments.Length == 0)
            {
                return builder;
            }
            builder.Append("<");
            for (int i = 0; i < typeArguments.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }
                builder.Append(typeArguments[i].ToDisplayString());
            }
            builder.Append(">");
            return builder;
        }

        public static StringBuilder AppendType(this StringBuilder builder, ITypeSymbol type, ref bool requireNullable)
        {
            switch (type)
            {
                case IArrayTypeSymbol arrayType:
                    builder.AppendType(arrayType.ElementType, ref requireNullable).Append("[]");
                    break;
                case IPointerTypeSymbol pointerType:
                    builder.AppendType(pointerType.PointedAtType, ref requireNullable).Append("*");
                    break;
                case INamedTypeSymbol { IsTupleType: true } tupleType:
                    builder.AppendTupleType(tupleType, ref requireNullable);
                    break;
                case INamedTypeSymbol { IsTupleType: false } namedType:
                    builder.AppendNamedType(namedType, ref requireNullable);
                    break;
                case IDynamicTypeSymbol:
                    builder.Append("dynamic");
                    break;
                case IFunctionPointerTypeSymbol funcPtr:
                    builder.Append(funcPtr.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                    break;
                default:
                    builder.Append(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                    break;
            }
            requireNullable = requireNullable || type.NullableAnnotation == NullableAnnotation.Annotated;
            return builder;
        }
        private static void AppendTupleType(this StringBuilder builder, INamedTypeSymbol tupleType, ref bool requireNullable)
        {
            var elements = tupleType.TupleElements;

            builder.Append("(");
            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                builder.AppendType(element.Type, ref requireNullable);
                if (!string.IsNullOrEmpty(element.Name))
                {
                    builder.Append(" ").Append(element.Name);
                }

                if (i < elements.Length - 1)
                    builder.Append(", ");
            }
            builder.Append(")");
        }
        private static void AppendNamedType(this StringBuilder builder, INamedTypeSymbol typeSymbol, ref bool requireNullable)
        {
            if (
                typeSymbol.SpecialType != SpecialType.None
                && typeSymbol.SpecialType != SpecialType.System_DateTime
            )
            {
                builder.Append(typeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                return;
            }

            if (typeSymbol.ContainingType != null)
            {
                builder.AppendNamedType(typeSymbol.ContainingType, ref requireNullable);
                builder.Append(".");
            }
            else if (typeSymbol.ContainingNamespace != null &&
                !typeSymbol.ContainingNamespace.IsGlobalNamespace)
            {
                builder.Append("global::").Append(typeSymbol.ContainingNamespace.ToDisplayString()).Append(".");
            }


            builder.Append(typeSymbol.Name);

            if (typeSymbol.TypeArguments.Length > 0)
            {
                builder.Append("<");
                for (int i = 0; i < typeSymbol.TypeArguments.Length; i++)
                {
                    builder.AppendType(typeSymbol.TypeArguments[i], ref requireNullable);
                    if (i < typeSymbol.TypeArguments.Length - 1)
                        builder.Append(", ");
                }
                builder.Append(">");
            }

            if (typeSymbol.Name != "Nullable" && typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
            {
                builder.Append("?");
            }
        }

        public static StringBuilder AppendInheritdoc(this StringBuilder builder, ITypeSymbol type, ref bool requireNullable)
        {
            builder.Append("/// <inheritdoc cref=\"").AppendType(type, ref requireNullable).Append("\"/>");
            return builder;
        }

        public static StringBuilder AppendInheritdoc(this StringBuilder builder, ITypeSymbol type, string memberName, ref bool requireNullable)
        {
            builder.Append("/// <inheritdoc cref=\"").AppendType(type, ref requireNullable).Append(".").Append(memberName).Append("\"/>");
            return builder;
        }


        public static StringBuilder AppendAttributes(this StringBuilder builder, ImmutableArray<AttributeData> attributes, ref bool requireNullable, Action<StringBuilder>? appendSuffix = null)
        {
            if (attributes.Length == 0)
            {
                return builder;
            }
            var lastIndex = builder.Length;
            var appendedAttribute = false;
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeClass is null)
                {
                    continue;
                }
                var displayString = attribute.AttributeClass.ToDisplayString();
                if (displayString.StartsWith("System.Runtime.CompilerServices.") || displayString == "System.Diagnostics.ConditionalAttribute")
                {
                    continue;
                }
                if (attribute.AttributeClass.DeclaredAccessibility != Accessibility.Public)
                {
                    continue;
                }

                if (appendedAttribute)
                {
                    builder.Append(", ");
                }
                appendedAttribute = true;

                builder.AppendType(attribute.AttributeClass, ref requireNullable).Append("(");

                var appendedArg = false;
                foreach (var ctorArg in attribute.ConstructorArguments)
                {
                    if (appendedArg)
                    {
                        builder.Append(", ");
                    }
                    appendedArg = true;

                    builder.AppendTypedConstant(ctorArg, ref requireNullable);
                }

                foreach (var NamedArg in attribute.NamedArguments)
                {
                    if (appendedArg)
                    {
                        builder.Append(", ");
                    }
                    appendedArg = true;

                    builder.Append(NamedArg.Key).Append(" = ").AppendTypedConstant(NamedArg.Value, ref requireNullable);
                }

                builder.Append(")");
            }
            if (appendedAttribute)
            {
                builder.Insert(lastIndex, "[");
                builder.Append("] ");

                appendSuffix?.Invoke(builder);
            }

            return builder;
        }

        private static StringBuilder AppendTypedConstant(this StringBuilder builder, TypedConstant constant, ref bool requireNullable)
        {
            if (constant.IsNull)
            {
                builder.Append("null");
                return builder;
            }

            if (constant.Kind == TypedConstantKind.Array)
            {
                builder.Append("{");
                for (int i = 0; i < constant.Values.Length; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(", ");
                    }
                    builder.AppendTypedConstant(constant.Values[i], ref requireNullable);
                }
                builder.Append("}");
                return builder;
            }

            if (constant.Kind == TypedConstantKind.Type || constant.Type!.SpecialType == SpecialType.System_Object)
            {
                builder.Append("typeof(").AppendType(constant.Type!, ref requireNullable).Append(")");
                return builder;
            }

            if (constant.Kind == TypedConstantKind.Enum)
            {
                builder.Append("global::");

            }

            builder.Append(constant.ToCSharpString());

            return builder;
        }
    }
}