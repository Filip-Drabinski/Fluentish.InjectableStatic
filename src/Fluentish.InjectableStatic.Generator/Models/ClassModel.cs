using Fluentish.InjectableStatic.Generator.Models.Members;
using Fluentish.InjectableStatic.Generator.Models.Metadata;
using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models
{
    [DebuggerDisplay("{OriginalTypeFullName, nq} | GenericArguments[{GenericArguments.Length}], Properties[{Properties.Length,000}], Events[{Events.Length,000}], Fields[{Fields.Length,000}], Methods[{Methods.Length,000}],")]
    public class ClassModel
    {
        public GenericArgumentModel[] GenericArguments { get; set; }
        public string OriginalTypeFullName { get; set; }
        public string? Namespace { get; set; }
        public string Name { get; set; }
        public bool IsUnsafe { get; set; }

        public PropertyModel[] Properties { get; set; }
        public EventModel[] Events { get; set; }
        public FieldModel[] Fields { get; set; }
        public MethodModel[] Methods { get; set; }

        public bool RequireNullable { get; set; }
        public string EndLine { get; set; }

        public ClassModel(
            GenericArgumentModel[] genericArguments,
            string originalTypeFullName,
            string @namespace,
            string name,
            bool isUnsafe,
            PropertyModel[] properties,
            EventModel[] events,
            FieldModel[] fields,
            MethodModel[] methods,
            bool requireNullable,
            string endLine
        )
        {
            GenericArguments = genericArguments;
            OriginalTypeFullName = originalTypeFullName;
            Namespace = @namespace;
            Name = name;
            IsUnsafe = isUnsafe;
            Properties = properties;
            Events = events;
            Fields = fields;
            Methods = methods;

            RequireNullable = requireNullable;
            EndLine = endLine;
        }
    }
}
