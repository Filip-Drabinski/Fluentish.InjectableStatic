using Fluentish.InjectableStatic.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Fluentish.InjectableStatic.Generator.MemberBuilders
{
    internal static class MethodMemberBuilder
    {
        public static bool TryAppend(INamedTypeSymbol type, ISymbol symbol, StringBuilder interfaceBuilder, StringBuilder implementationBuilder, string newLineSymbol, ref bool requireNullable, int baseIndentation)
        {
            if (
                symbol is not IMethodSymbol methodSymbol
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
                return false;
            }

            requireNullable =
                requireNullable
                || methodSymbol.ReturnType.NullableAnnotation == NullableAnnotation.Annotated
                || methodSymbol.TypeArgumentNullableAnnotations.Any(x => x == NullableAnnotation.Annotated)
                || methodSymbol.ReturnNullableAnnotation == NullableAnnotation.Annotated
                || methodSymbol.ReceiverNullableAnnotation == NullableAnnotation.Annotated;

            var returnTypeName = methodSymbol.ReturnType.ToDisplayString();


            interfaceBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, methodSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("[global::System.Diagnostics.DebuggerStepThrough]").Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]").Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(methodSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1))
                .AppendType(methodSymbol.ReturnType, ref requireNullable).Append(" ").Append(methodSymbol.Name).AppendTypeArguments(methodSymbol.TypeArguments).Append("(");

            implementationBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, methodSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("[global::System.Diagnostics.DebuggerStepThrough]").Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]").Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(methodSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1))
                .AppendAccessibility(methodSymbol.DeclaredAccessibility).AppendType(methodSymbol.ReturnType, ref requireNullable).Append(" ").Append(methodSymbol.Name).AppendTypeArguments(methodSymbol.TypeArguments).Append("(");

            for (int i = 0; i < methodSymbol.Parameters.Length; i++)
            {
                requireNullable = requireNullable || methodSymbol.Parameters[i].NullableAnnotation == NullableAnnotation.Annotated;

                if (i > 0)
                {
                    interfaceBuilder.Append(", ");
                    implementationBuilder.Append(", ");
                }

                interfaceBuilder.AppendAttributes(methodSymbol.Parameters[i].GetAttributes(), ref requireNullable, b => b.Append(" "));
                implementationBuilder.AppendAttributes(methodSymbol.Parameters[i].GetAttributes(), ref requireNullable, b => b.Append(" "));

                if (methodSymbol.Parameters[i].IsParams)
                {
                    interfaceBuilder.Append("params ");
                    implementationBuilder.Append("params ");
                }

                interfaceBuilder.AppendReferenceKind(methodSymbol.Parameters[i].RefKind);
                implementationBuilder.AppendReferenceKind(methodSymbol.Parameters[i].RefKind);

                interfaceBuilder.AppendType(methodSymbol.Parameters[i].Type, ref requireNullable).Append(" ").Append(methodSymbol.Parameters[i].Name);
                implementationBuilder.AppendType(methodSymbol.Parameters[i].Type, ref requireNullable).Append(" ").Append(methodSymbol.Parameters[i].Name);
            }

            if (methodSymbol.TypeArguments.Any())
            {
                interfaceBuilder
                    .Append(")").AppendTypeConstraints(methodSymbol.TypeArguments, methodSymbol.OriginalDefinition.TypeParameters, baseIndentation + 1, newLineSymbol, ref requireNullable).Append(";")
                    .Append(newLineSymbol);
            }
            else
            {
                interfaceBuilder
                    .Append(");").Append(newLineSymbol);
            }

            if (methodSymbol.TypeArguments.Any())
            {
                implementationBuilder
                    .Append(")").AppendTypeConstraints(methodSymbol.TypeArguments, methodSymbol.OriginalDefinition.TypeParameters, baseIndentation + 1, newLineSymbol, ref requireNullable);
            }
            else
            {
                implementationBuilder
                    .Append(")");
            }


            implementationBuilder.Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 2).Append(" => ").AppendType(type, ref requireNullable).Append(".").Append(methodSymbol.Name).AppendTypeArguments(methodSymbol.TypeArguments).Append("(");

            for (int i = 0; i < methodSymbol.Parameters.Length; i++)
            {
                if (i > 0)
                {
                    implementationBuilder.Append(", ");
                }

                implementationBuilder.AppendReferenceKind(methodSymbol.Parameters[i].RefKind);

                implementationBuilder.Append(methodSymbol.Parameters[i].Name);
            }

            implementationBuilder
                .Append(");").Append(newLineSymbol);

            return true;
        }

    }
}