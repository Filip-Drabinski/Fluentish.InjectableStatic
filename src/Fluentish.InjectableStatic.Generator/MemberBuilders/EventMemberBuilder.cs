using Fluentish.InjectableStatic.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Fluentish.InjectableStatic.Generator.MemberBuilders
{
    internal static class EventMemberBuilder
    {
        public static bool TryAppend(INamedTypeSymbol type, ISymbol symbol, StringBuilder interfaceBuilder, StringBuilder implementationBuilder, string newLineSymbol, ref bool requireNullable, out string name, int baseIndentation)
        {
            name = symbol.Name;

            if (symbol is not IEventSymbol eventSymbol)
            {
                return false;
            }

            interfaceBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, eventSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(eventSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1)).Append("event ").AppendType(eventSymbol.Type, ref requireNullable).Append(" ").Append(eventSymbol.Name).Append(";").Append(newLineSymbol);

            implementationBuilder
                .AppendIndentation(baseIndentation + 1).AppendInheritdoc(type, eventSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).AppendAttributes(eventSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(baseIndentation + 1)).AppendAccessibility(eventSymbol.DeclaredAccessibility).Append("event ").AppendType(eventSymbol.Type, ref requireNullable).Append(" ").Append(eventSymbol.Name)
                .Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("{");

            if (eventSymbol.AddMethod is not null && eventSymbol.AddMethod.DeclaredAccessibility == Accessibility.Public)
            {
                implementationBuilder.Append(newLineSymbol)
                    .AppendIndentation(3).Append("add => ").AppendType(type, ref requireNullable).Append(".").Append(eventSymbol.Name).Append(" += value;");
            }

            if (eventSymbol.RemoveMethod is not null && eventSymbol.RemoveMethod.DeclaredAccessibility == Accessibility.Public)
            {
                implementationBuilder.Append(newLineSymbol)
                    .AppendIndentation(3).Append("remove => ").AppendType(type, ref requireNullable).Append(".").Append(eventSymbol.Name).Append(" -= value;");
            }

            implementationBuilder.Append(newLineSymbol)
                .AppendIndentation(baseIndentation + 1).Append("}").Append(newLineSymbol);

            return true;
        }
    }
}