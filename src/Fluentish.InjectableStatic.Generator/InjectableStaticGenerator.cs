using Fluentish.InjectableStatic.Generator.Attributes;
using Fluentish.InjectableStatic.Generator.Extensions;
using Fluentish.InjectableStatic.Generator.MemberBuilders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Fluentish.InjectableStatic.Generator
{
    internal enum FilterType
    {
        Exclude = 0,
        Include = 1
    }

    [Generator]
    public class InjectableStaticGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {

            context.RegisterPostInitializationOutput(context =>
            {
                context.AddInjectableStaticAttribute();
                context.AddInjectableNamespacePrefixAttribute();
            });

            var namespacePrefixProvider = context.CompilationProvider
                .SelectMany((compilation, ct) =>
                {
                    var attributes = compilation.Assembly.GetAttributes();

                    var injectableAttributeSymbol = compilation.GetTypeByMetadataName("Fluentish.InjectableStatic.InjectableNamespacePrefixAttribute");

                    var matchingAttributes = attributes
                        .Where(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, injectableAttributeSymbol));

                    var selectedTypes = matchingAttributes.Select(attribute =>
                    {
                        var targetTypeArgument = attribute.ConstructorArguments.First();
                        var namespacePrefixValue = targetTypeArgument.Value!.ToString();

                        return namespacePrefixValue;
                    });

                    return selectedTypes;
                })
                .Collect();

            var typesToMakeInjectable = context.CompilationProvider
                .SelectMany((compilation, ct) =>
                {
                    var attributes = compilation.Assembly.GetAttributes();

                    var injectableAttributeSymbol = compilation.GetTypeByMetadataName("Fluentish.InjectableStatic.InjectableAttribute");

                    var matchingAttributes = attributes
                        .Where(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, injectableAttributeSymbol));

                    var selectedTypes = matchingAttributes
                        .Select(attribute =>
                        {
                            if (attribute.ConstructorArguments.Length == 1)
                            {
                                var targetTypeArgument = attribute.ConstructorArguments[0];
                                var targetTypeName = targetTypeArgument.Value!.ToString();

                                var injectableAttributeSymbol = compilation.GetTypeByMetadataName(targetTypeName);

                                return (injectableAttributeSymbol, FilterType.Exclude, System.Array.Empty<string>());
                            }
                            else if (attribute.ConstructorArguments.Length == 3)
                            {

                                var targetTypeArgument = attribute.ConstructorArguments[0];
                                var targetTypeName = targetTypeArgument.Value!.ToString();

                                var filterKind = (FilterType)attribute.ConstructorArguments[1].Value!;
                                var filteredTypes = attribute.ConstructorArguments[2].Values.Select(x => x.Value?.ToString()).ToArray();

                                var injectableAttributeSymbol = compilation.GetTypeByMetadataName(targetTypeName);

                                return (injectableAttributeSymbol, filterKind, filteredTypes);
                            }
                            return default;
                        })
                        .Where(x => x != default);

                    return selectedTypes;
                })
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Combine(namespacePrefixProvider);

            context.RegisterSourceOutput(typesToMakeInjectable, (sourceProductionContext, value) =>
            {
                var (left, namespacePrefixes) = value;
                var (data, optionsProvider) = left;
                var namespacePrefix = namespacePrefixes.FirstOrDefault();

                GenerateInjectable(sourceProductionContext, data!, optionsProvider, namespacePrefix);
            });
        }

        private static void GenerateInjectable(SourceProductionContext SourceProductionContext, (INamedTypeSymbol type, FilterType filter, string[] members) data, AnalyzerConfigOptionsProvider optionsProvider, string? namespacePrefix)
        {
            var newLineSymbol = optionsProvider.GlobalOptions.GetNewLineSymbol();

            namespacePrefix ??= "Fluentish.Injectable.";

            if (string.IsNullOrWhiteSpace(namespacePrefix))
            {
                namespacePrefix = "";
            }
            else if (!namespacePrefix.EndsWith("."))
            {
                namespacePrefix += ".";
            }

            if (data.type is null)
            {
                return;
            }

            var typeModifiers = data.type.DeclaringSyntaxReferences.Select(x => x.GetSyntax()).Cast<TypeDeclarationSyntax>().SelectMany(x => x.Modifiers);

            var isUnsafe = typeModifiers.Any(x => x.IsKind(SyntaxKind.UnsafeKeyword));

            var requireNullable = false;
            var typeFullName = data.type.ToDisplayString();
            var @namespace = data.type.ContainingNamespace.ToDisplayString();

            var interfaceHint = $"I{data.type.Name}.g.cs";
            var interfaceBuilder = new StringBuilder()
                .Append("#pragma warning disable").Append(newLineSymbol)
                .Append("namespace ").Append(namespacePrefix).Append(@namespace).Append(newLineSymbol)
                .Append("{").Append(newLineSymbol)
                .AppendIndentation().AppendInheritdoc(data.type, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation().Append("public ").Append(isUnsafe ? "unsafe " : "").Append("interface I").Append(data.type.Name).Append(newLineSymbol)
                .AppendIndentation().Append("{").Append(newLineSymbol);

            var implementationHint = $"{data.type.Name}.g.cs";
            var implementationBuilder = new StringBuilder()
                .Append("#pragma warning disable").Append(newLineSymbol)
                .Append("namespace ").Append(namespacePrefix).Append(@namespace).Append(newLineSymbol)
                .Append("{").Append(newLineSymbol)
                .AppendIndentation().AppendInheritdoc(data.type, ref requireNullable).Append(newLineSymbol)
                .AppendIndentation().Append("[global::System.Diagnostics.DebuggerStepThrough]").Append(newLineSymbol)
                .AppendIndentation().Append("public ").Append(isUnsafe ? "unsafe " : "").Append("class ").Append(data.type.Name).Append("Service").Append(": I").Append(data.type.Name).Append(newLineSymbol)
                .AppendIndentation().Append("{").Append(newLineSymbol);

            var allMembers = data.type.GetMembers();

            for (int memberIndex = 0; memberIndex < allMembers.Length; memberIndex++)
            {
                ISymbol? memberSymbol = allMembers[memberIndex];

                if (memberSymbol.DeclaredAccessibility != Accessibility.Public
                    || !memberSymbol.IsStatic)
                {
                    continue;
                }
                if(data.filter == FilterType.Exclude && data.members.Contains(memberSymbol.Name))
                {
                    continue;
                }
                if(data.filter == FilterType.Include && !data.members.Contains(memberSymbol.Name))
                {
                    continue;
                }
                var generatedMember = false;

                if (EventMemberBuilder.TryAppend(data.type, memberSymbol, interfaceBuilder, implementationBuilder, newLineSymbol, ref requireNullable, out var eventName))
                {
                    generatedMember = true;
                }
                else if (PropertyMemberBuilder.TryAppend(data.type, memberSymbol, interfaceBuilder, implementationBuilder, newLineSymbol, ref requireNullable, out var propertyName))
                {
                    generatedMember = true;
                }
                else if (FieldMemberBuilder.TryAppend(data.type, memberSymbol, interfaceBuilder, implementationBuilder, newLineSymbol, ref requireNullable))
                {
                    generatedMember = true;
                }
                else if (MethodMemberBuilder.TryAppend(data.type, memberSymbol, interfaceBuilder, implementationBuilder, newLineSymbol, ref requireNullable))
                {
                    generatedMember = true;
                }

                if (generatedMember && memberIndex < allMembers.Length - 1)
                {
                    interfaceBuilder.Append(newLineSymbol);
                    implementationBuilder.Append(newLineSymbol);
                }

            }

            interfaceBuilder
                .Append("    }").Append(newLineSymbol)
                .Append("}").Append(newLineSymbol)
                .Append("#pragma warning restore").Append(newLineSymbol);

            implementationBuilder
                .Append("    }").Append(newLineSymbol)
                .Append("}").Append(newLineSymbol)
                .Append("#pragma warning restore").Append(newLineSymbol);

            if (requireNullable)
            {
                interfaceBuilder.Insert(0, newLineSymbol).Insert(0, "#nullable enable");
                implementationBuilder.Insert(0, newLineSymbol).Insert(0, "#nullable enable");
            }

            interfaceBuilder.Insert(0, newLineSymbol).Insert(0, "// <auto-generated />");
            implementationBuilder.Insert(0, newLineSymbol).Insert(0, "// <auto-generated />");

            var interfaceSource = interfaceBuilder.ToString();
            var implementationSource = implementationBuilder.ToString();

            SourceProductionContext.AddSource(interfaceHint, interfaceSource);
            SourceProductionContext.AddSource(implementationHint, implementationSource);
        }
    }
}