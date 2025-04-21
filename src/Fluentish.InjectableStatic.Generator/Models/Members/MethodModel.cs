using Fluentish.InjectableStatic.Generator.Extensions;
using Fluentish.InjectableStatic.Generator.Models.Metadata;
using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models.Members
{
    [DebuggerDisplay("{ReturnType,nq} {Name,nq} | Parameters[{Parameters.Length}], Attributes[{Attributes.Length}], GenericArguments[{GenericArguments.Length}]")]
    public class MethodModel
    {
        public AttributeModel[] Attributes { get; set; }
        public string ReturnType { get; set; }
        public string Name { get; set; }
        public GenericArgumentModel[] GenericArguments { get; set; }
        public MethodParameterModel[] Parameters { get; set; }

        public MethodModel(
            AttributeModel[] attributes,
            string returnType,
            string name,
            GenericArgumentModel[] genericArguments,
            MethodParameterModel[] parameters
        )
        {
            Attributes = attributes;
            GenericArguments = genericArguments;
            ReturnType = returnType;
            Name = name.EscapeKeyword();
            Parameters = parameters;
        }
    }
}
