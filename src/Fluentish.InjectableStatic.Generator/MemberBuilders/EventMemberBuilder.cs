using Fluentish.InjectableStatic.Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Fluentish.InjectableStatic.Generator.MemberBuilders
{
    internal static class EventMemberBuilder
    {
        public static bool TryAppend(INamedTypeSymbol type, ISymbol symbol, StringBuilder interfaceBuilder, StringBuilder implementationBuilder, string newLineSymbol, ref bool requireNullable, out string name)
        {
            name = symbol.Name;

            if (symbol is not IEventSymbol eventSymbol)
            {
                return false;
            }

            interfaceBuilder
                .AppendIndentation(2).AppendInheritdoc(type, eventSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(2).AppendAttributes(eventSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(2)).Append("event ").AppendType(eventSymbol.Type, ref requireNullable).Append(" ").Append(eventSymbol.Name).Append(";").Append(newLineSymbol);

            implementationBuilder
                .AppendIndentation(2).AppendInheritdoc(type, eventSymbol.Name, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation(2).AppendAttributes(eventSymbol.GetAttributes(), ref requireNullable, b => b.Append(newLineSymbol).AppendIndentation(2)).AppendAccessibility(eventSymbol.DeclaredAccessibility).Append("event ").AppendType(eventSymbol.Type, ref requireNullable).Append(" ").Append(eventSymbol.Name)
                .Append(newLineSymbol)
                .AppendIndentation(2).Append("{");

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
                .AppendIndentation(2).Append("}").Append(newLineSymbol);

            return true;
        }
    }
}