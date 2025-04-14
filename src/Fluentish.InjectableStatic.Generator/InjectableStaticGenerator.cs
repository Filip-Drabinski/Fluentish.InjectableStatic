using Fluentish.InjectableStatic.Generator.Attributes;
using Fluentish.InjectableStatic.Generator.Extensions;
using Fluentish.InjectableStatic.Generator.MemberBuilders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text;
namespace Fluentish.InjectableStatic.Generator
{
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
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Select((data, ct) =>
                {
                    var (compilation, optionsProvider) = data;

                    var newLineSymbol = optionsProvider.GlobalOptions.GetNewLineSymbol();

                    var attributes = compilation.Assembly.GetAttributes();

                    var injectableAttributeSymbol = compilation.GetTypeByMetadataName("Fluentish.InjectableStatic.InjectableNamespacePrefixAttribute");

                    var injectableNamespacePrefixAttribute = attributes
                        .FirstOrDefault(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, injectableAttributeSymbol));

                    if(injectableNamespacePrefixAttribute is null)
                    {
                        return new InjectableStaticConfiguration(
                            @namespace: null,
                            endLine: newLineSymbol
                        );
                    }

                    var targetTypeArgument = injectableNamespacePrefixAttribute.ConstructorArguments.First();
                    var namespacePrefixValue = targetTypeArgument.Value!.ToString();

                    return new InjectableStaticConfiguration(
                        namespacePrefixValue,
                        endLine: newLineSymbol
                    );
                });

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

                                if(injectableAttributeSymbol is null)
                                {
                                    return null;
                                }

                                return new InjectableClassInfo(injectableAttributeSymbol, FilterType.Exclude, []);
                            }
                            else if (attribute.ConstructorArguments.Length == 3)
                            {

                                var targetTypeArgument = attribute.ConstructorArguments[0];
                                var targetTypeName = targetTypeArgument.Value!.ToString();

                                var filterKind = (FilterType)attribute.ConstructorArguments[1].Value!;
                                var filteredTypes = attribute.ConstructorArguments[2].Values.Where(x=>!x.IsNull).Select(x => x.Value!.ToString()).ToArray();

                                var injectableAttributeSymbol = compilation.GetTypeByMetadataName(targetTypeName);

                                if (injectableAttributeSymbol is null)
                                {
                                    return null;
                                }

                                return new InjectableClassInfo(injectableAttributeSymbol, filterKind, filteredTypes ?? []);
                            }
                            return default;
                        })
                        .Where(x => x != default);

                    return selectedTypes;
                })
                .Combine(namespacePrefixProvider);

            context.RegisterSourceOutput(typesToMakeInjectable, (sourceProductionContext, value) =>
            {
                var (data, configuration) = value;

                GenerateInjectable(sourceProductionContext, data!, configuration);
            });
        }

        private static void GenerateInjectable(SourceProductionContext SourceProductionContext, InjectableClassInfo classInfo, InjectableStaticConfiguration configuration)
        {

            if (classInfo.type is null)
            {
                return;
            }

            var typeModifiers = classInfo.type.DeclaringSyntaxReferences.Select(x => x.GetSyntax()).Cast<TypeDeclarationSyntax>().SelectMany(x => x.Modifiers);

            var isUnsafe = typeModifiers.Any(x => x.IsKind(SyntaxKind.UnsafeKeyword));

            var requireNullable = false;
            var typeFullName = classInfo.type.ToDisplayString();
            var @namespace = classInfo.type.ContainingNamespace.ToDisplayString();

            var interfaceHint = $"I{classInfo.type.Name}.g.cs";
            var interfaceBuilder = new StringBuilder()
                .Append("#pragma warning disable").Append(configuration.EndLine)
                .Append("namespace ").Append(configuration.Namespace).Append(@namespace).Append(configuration.EndLine)
                .Append("{").Append(configuration.EndLine)
                .AppendIndentation().AppendInheritdoc(classInfo.type, ref requireNullable).Append(configuration.EndLine)
                .AppendIndentation().Append("public ").Append(isUnsafe ? "unsafe " : "").Append("interface I").Append(classInfo.type.Name).Append(configuration.EndLine)
                .AppendIndentation().Append("{").Append(configuration.EndLine);

            var implementationHint = $"{classInfo.type.Name}.g.cs";
            var implementationBuilder = new StringBuilder()
                .Append("#pragma warning disable").Append(configuration.EndLine)
                .Append("namespace ").Append(configuration.Namespace).Append(@namespace).Append(configuration.EndLine)
                .Append("{").Append(configuration.EndLine)
                .AppendIndentation().AppendInheritdoc(classInfo.type, ref requireNullable).Append(configuration.EndLine)
                .AppendIndentation().Append("[global::System.Diagnostics.DebuggerStepThrough]").Append(configuration.EndLine)
                .AppendIndentation().Append("public ").Append(isUnsafe ? "unsafe " : "").Append("class ").Append(classInfo.type.Name).Append("Service").Append(": I").Append(classInfo.type.Name).Append(configuration.EndLine)
                .AppendIndentation().Append("{").Append(configuration.EndLine);

            var allMembers = classInfo.type.GetMembers();

            for (int memberIndex = 0; memberIndex < allMembers.Length; memberIndex++)
            {
                ISymbol? memberSymbol = allMembers[memberIndex];

                if (memberSymbol.DeclaredAccessibility != Accessibility.Public
                    || !memberSymbol.IsStatic)
                {
                    continue;
                }
                if(classInfo.filter == FilterType.Exclude && classInfo.members.Contains(memberSymbol.Name))
                {
                    continue;
                }
                if(classInfo.filter == FilterType.Include && !classInfo.members.Contains(memberSymbol.Name))
                {
                    continue;
                }
                var generatedMember = false;

                if (EventMemberBuilder.TryAppend(classInfo.type, memberSymbol, interfaceBuilder, implementationBuilder, configuration.EndLine, ref requireNullable, out var eventName))
                {
                    generatedMember = true;
                }
                else if (PropertyMemberBuilder.TryAppend(classInfo.type, memberSymbol, interfaceBuilder, implementationBuilder, configuration.EndLine, ref requireNullable, out var propertyName))
                {
                    generatedMember = true;
                }
                else if (FieldMemberBuilder.TryAppend(classInfo.type, memberSymbol, interfaceBuilder, implementationBuilder, configuration.EndLine, ref requireNullable))
                {
                    generatedMember = true;
                }
                else if (MethodMemberBuilder.TryAppend(classInfo.type, memberSymbol, interfaceBuilder, implementationBuilder, configuration.EndLine, ref requireNullable))
                {
                    generatedMember = true;
                }

                if (generatedMember && memberIndex < allMembers.Length - 1)
                {
                    interfaceBuilder.Append(configuration.EndLine);
                    implementationBuilder.Append(configuration.EndLine);
                }

            }

            interfaceBuilder
                .Append("    }").Append(configuration.EndLine)
                .Append("}").Append(configuration.EndLine)
                .Append("#pragma warning restore").Append(configuration.EndLine);

            implementationBuilder
                .Append("    }").Append(configuration.EndLine)
                .Append("}").Append(configuration.EndLine)
                .Append("#pragma warning restore").Append(configuration.EndLine);

            if (requireNullable)
            {
                interfaceBuilder.Insert(0, configuration.EndLine).Insert(0, "#nullable enable");
                implementationBuilder.Insert(0, configuration.EndLine).Insert(0, "#nullable enable");
            }

            interfaceBuilder.Insert(0, configuration.EndLine).Insert(0, "// <auto-generated />");
            implementationBuilder.Insert(0, configuration.EndLine).Insert(0, "// <auto-generated />");

            var interfaceSource = interfaceBuilder.ToString();
            var implementationSource = implementationBuilder.ToString();

            SourceProductionContext.AddSource(interfaceHint, interfaceSource);
            SourceProductionContext.AddSource(implementationHint, implementationSource);
        }
    }
}