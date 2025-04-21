using Fluentish.InjectableStatic.Generator.Models.Metadata;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Fluentish.InjectableStatic.Generator.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendInheridoc(this StringBuilder builder, string typeFullName)
        {
            builder.Append("/// <inheritdoc cref=\"").Append(typeFullName).Append("\"/>");
            return builder;
        }
        public static StringBuilder AppendInheridoc(this StringBuilder builder, string typeFullName, string memberName)
        {
            builder.Append("/// <inheritdoc cref=\"").Append(typeFullName).Append(".").Append(memberName).Append("\"/>");
            return builder;
        }

        public static StringBuilder AppendTypeGenericArguments(this StringBuilder builder, GenericArgumentModel[] genericArguments)
        {
            if (genericArguments.Length == 0)
            {
                return builder;
            }

            builder
                .Append("<");

            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (i > 0)
                {
                    builder
                        .Append(", ");
                }

                builder
                    .Append(genericArguments[i].Name);
            }

            builder
                .Append(">");

            return builder;
        }

        public static StringBuilder AppendTypeGenericConstraint(this StringBuilder builder, GenericArgumentModel[] genericArguments, int indentationDepth, string endLine)
        {
            if (genericArguments.Length == 0)
            {
                return builder;
            }

            var lastIndex = builder.Length;

            var isFirst = true;
            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (genericArguments[i].Constraints is null)
                {
                    continue;
                }

                var constraints = genericArguments[i].Constraints!;
                if (
                    !constraints.HasReferenceTypeConstraint
                    && !constraints.HasValueTypeConstraint
                    && !constraints.HasUnmanagedTypeConstraint
                    && constraints.TypeConstraints.Length == 0
                    && !constraints.HasConstructorConstraint
                )
                {
                    continue;
                }

                if (!isFirst)
                {
                    builder
                        .Append(endLine);
                }

                builder
                    .AppendIndentation(indentationDepth).AppendTypeGenericConstraint(genericArguments[i].Name, constraints);

                isFirst = false;
            }

            if (!isFirst)
            {
                builder.Insert(lastIndex, endLine);
            }

            return builder;
        }

        public static StringBuilder AppendTypeGenericConstraint(this StringBuilder builder, string argumentName, GenericArgumentConstraintsModel constraints)
        {
            builder
                .Append("where ").Append(argumentName).Append(" : ");

            if (constraints.HasReferenceTypeConstraint)
            {
                builder
                    .Append("class");
            }

            if (constraints.HasValueTypeConstraint)
            {
                builder
                    .AppendIf(constraints.HasReferenceTypeConstraint, ", ").Append("struct");
            }

            if (constraints.HasUnmanagedTypeConstraint)
            {
                builder
                .AppendIf(constraints.HasReferenceTypeConstraint || constraints.HasValueTypeConstraint, ", ").Append("unmanaged");
            }

            for (int j = 0; j < constraints.TypeConstraints.Length; j++)
            {
                builder
                    .AppendIf(j > 0 || constraints.HasReferenceTypeConstraint || constraints.HasValueTypeConstraint || constraints.HasUnmanagedTypeConstraint, ", ")
                    .Append(constraints.TypeConstraints[j]);
            }

            if (constraints.HasConstructorConstraint)
            {
                builder
                    .AppendIf(constraints.HasValueTypeConstraint || constraints.HasValueTypeConstraint || constraints.HasReferenceTypeConstraint || constraints.TypeConstraints.Length > 0, ", ").Append("new()");
            }

            return builder;
        }

        public static StringBuilder AppendAttributes(this StringBuilder builder, AttributeModel[] attributes, int indentationDepth, string endLine)
        {
            if (attributes.Length == 0)
            {
                return builder;
            }

            foreach (var attribute in attributes)
            {
                builder
                    .AppendIndentation(indentationDepth).Append("[").AppendAttribute(attribute.TypeFullName, attribute.Arguments, attribute.NamedArguments).Append("]").Append(endLine);
            }
            return builder;
        }

        public static StringBuilder AppendAttributes(this StringBuilder builder, AttributeModel[] attributes)
        {
            if (attributes.Length == 0)
            {
                return builder;
            }
            builder
                .Append("[");

            for (int i = 0; i < attributes.Length; i++)
            {
                if (i > 0)
                {
                    builder
                        .Append(", ");
                }

                builder
                    .AppendAttribute(attributes[i].TypeFullName, attributes[i].Arguments, attributes[i].NamedArguments);
            }

            builder
                .Append("] ");
            return builder;
        }

        public static StringBuilder AppendAttribute(
            this StringBuilder builder,
            string typeFullName,
            string[] arguments,
            (string name, string value)[] namedArguments
        )
        {
            builder
                .Append(typeFullName).Append("(");

            for (int i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                {
                    builder
                        .Append(", ");
                }

                builder
                    .Append(arguments[i]);
            }

            for (int i = 0; i < namedArguments.Length; i++)
            {
                if (
                    namedArguments.Length > 0
                    || i > 0
                )
                {
                    builder
                        .Append(", ");
                }

                builder
                    .Append(namedArguments[i].name).Append(" = ").Append(namedArguments[i].value);
            }

            builder
                .Append(")");

            return builder;
        }

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

        public static string EscapeKeyword(this string value)
        {
            if (Microsoft.CodeAnalysis.CSharp.SyntaxFacts.GetKeywordKind(value) == Microsoft.CodeAnalysis.CSharp.SyntaxKind.None)
            {
                return value;
            }

            return $"@{value}";
        }
    }
}