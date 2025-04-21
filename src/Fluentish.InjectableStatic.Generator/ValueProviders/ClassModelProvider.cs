using Fluentish.InjectableStatic.Generator.GeneratedAttributes;
using Fluentish.InjectableStatic.Generator.Models;
using Fluentish.InjectableStatic.Generator.Models.Members;
using Fluentish.InjectableStatic.Generator.ValueProviders.Mappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    internal static class ClassModelProvider
    {
        public static IncrementalValuesProvider<ClassModel> GetClassModelProvider(
            this IncrementalValuesProvider<(InjectableStaticInfo classInfo, InjectableStaticConfigurationInfo configuration)> injectableClassInfoProvider,
            TypeSerializer typeSerializer
        )
        {

            return injectableClassInfoProvider.Select((data, ct) =>
            {
                var (classInfo, configuration) = data;

                var typeModifiers = classInfo.type.DeclaringSyntaxReferences.Select(x => x.GetSyntax()).Cast<TypeDeclarationSyntax>().SelectMany(x => x.Modifiers);
                var isUnsafe = typeModifiers.Any(x => x.IsKind(SyntaxKind.UnsafeKeyword));



                var @namespace = configuration.NamespaceMode == NamespaceMode.Const
                    ? configuration.Namespace
                    : configuration.Namespace + classInfo.type.ContainingNamespace.ToDisplayString();

                var originalTypeFullName = typeSerializer.Serialize(classInfo.type, out var requireNullable);

                var genericArguments = classInfo.type.ToGenericArgumentModels(typeSerializer, out var genericTypesNullable);
                requireNullable |= genericTypesNullable;

                var propertyModels = new List<PropertyModel>();
                var eventModels = new List<EventModel>();
                var fieldModels = new List<FieldModel>();
                var methodModels = new List<MethodModel>();

                foreach (var memberSymbol in classInfo.type.GetMembers())
                {
                    if (
                        memberSymbol.DeclaredAccessibility != Accessibility.Public
                        || !memberSymbol.IsStatic
                    )
                    {
                        continue;
                    }

                    if (classInfo.filter == FilterType.Exclude && classInfo.members.Contains(memberSymbol.Name))
                    {
                        continue;
                    }
                    else if (classInfo.filter == FilterType.Include && !classInfo.members.Contains(memberSymbol.Name))
                    {
                        continue;
                    }

                    if (memberSymbol.TryParseEventModel(typeSerializer, out var eventModel, out var eventNullable))
                    {
                        requireNullable |= eventNullable;
                        eventModels.Add(eventModel);
                    }

                    if (memberSymbol.TryParsePropertyModel(typeSerializer, out var propertyModel, out var propertyNullable))
                    {
                        requireNullable |= propertyNullable;
                        propertyModels.Add(propertyModel);
                    }

                    if (memberSymbol.TryParseFieldModel(typeSerializer, out var fieldModel, out var fieldNullable))
                    {
                        requireNullable |= fieldNullable;
                        fieldModels.Add(fieldModel);
                    }

                    if (memberSymbol.TryParseMethodModel(typeSerializer, out var methodModel, out var methodNullable))
                    {
                        requireNullable |= methodNullable;
                        methodModels.Add(methodModel);
                    }

                }

                return new ClassModel(
                    genericArguments: genericArguments,
                    originalTypeFullName: originalTypeFullName,
                    @namespace: @namespace,
                    isUnsafe: isUnsafe,
                    name: classInfo.type.Name,
                    properties: propertyModels.ToArray(),
                    events: eventModels.ToArray(),
                    fields: fieldModels.ToArray(),
                    methods: methodModels.ToArray(),
                    requireNullable: requireNullable,
                    endLine: configuration.EndLine
                );
            });
        }
    }
}
