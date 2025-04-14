using Fluentish.InjectableStatic.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Fluentish.InjectableStatic.Generator.MemberBuilders
{
    internal static class FieldMemberBuilder
    {
        public static bool TryAppend(INamedTypeSymbol type, ISymbol symbol, StringBuilder interfaceBuilder, StringBuilder implementationBuilder, string newLineSymbol, ref bool requireNullable, int baseIndentation)
        {

            if (symbol is not IFieldSymbol fieldSymbol)
            {
                return false;
            }

            interfaceBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, fieldSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(fieldSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1)).AppendType(fieldSymbol.Type, ref requireNullable).Append(" ").Append(fieldSymbol.Name).Append(" { ");

            implementationBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, fieldSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(fieldSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1)).AppendAccessibility(fieldSymbol.DeclaredAccessibility).AppendType(fieldSymbol.Type, ref requireNullable).Append(" ").Append(fieldSymbol.Name)
                .Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("{");

            interfaceBuilder.Append("get; ");
            implementationBuilder.Append(newLineSymbol)
                .AppendIndentation(3).Append("get => ").AppendType(type, ref requireNullable).Append(".").Append(fieldSymbol.Name).Append(";");

            if (!fieldSymbol.IsConst && !fieldSymbol.IsReadOnly)
            {
                interfaceBuilder.Append("set; ");
                implementationBuilder.Append(newLineSymbol)
                    .AppendIndentation(3).Append("set => ").AppendType(type, ref requireNullable).Append(".").Append(fieldSymbol.Name).Append(" = value;");
            }

            interfaceBuilder.Append("}").Append(newLineSymbol)
                ;
            implementationBuilder.Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("}").Append(newLineSymbol)
                ;

            return true;
        }
    }
    internal static class PropertyMemberBuilder
    {
        public static bool TryAppend(INamedTypeSymbol type, ISymbol symbol, StringBuilder interfaceBuilder, StringBuilder implementationBuilder, string newLineSymbol, ref bool requireNullable, out string name, int baseIndentation)
        {
            name = symbol.Name;

            if (symbol is not IPropertySymbol propertySymbol)
            {
                return false;
            }

            interfaceBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, propertySymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(propertySymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1)).AppendType(propertySymbol.Type, ref requireNullable).Append(" ").Append(propertySymbol.Name).Append(" { ");

            implementationBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, propertySymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(propertySymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1)).AppendAccessibility(propertySymbol.DeclaredAccessibility).AppendType(propertySymbol.Type, ref requireNullable).Append(" ").Append(propertySymbol.Name)
                .Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("{");

            if(propertySymbol.GetMethod is not null && propertySymbol.GetMethod.DeclaredAccessibility == Accessibility.Public)
            {
                interfaceBuilder.Append("get; ");
                implementationBuilder.Append(newLineSymbol)
                    .AppendIndentation(3).Append("get => ").AppendType(type, ref requireNullable).Append(".").Append(propertySymbol.Name).Append(";");
            }

            if(propertySymbol.SetMethod is not null && propertySymbol.SetMethod.DeclaredAccessibility == Accessibility.Public)
            {
                interfaceBuilder.Append("set; ");
                implementationBuilder.Append(newLineSymbol)
                    .AppendIndentation(3).Append("set => ").AppendType(type, ref requireNullable).Append(".").Append(propertySymbol.Name).Append(" = value;");
            }

            interfaceBuilder.Append("}").Append(newLineSymbol)
                ;
            implementationBuilder.Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("}").Append(newLineSymbol)
                ;

            return true;
        }
    }
}