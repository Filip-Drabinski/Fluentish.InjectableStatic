using Fluentish.InjectableStatic.Generator.Extensions;
using Fluentish.InjectableStatic.Generator.Models.Metadata;
using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models.Members
{
    [DebuggerDisplay("{Type,nq} {Name,nq} | IsReadable[{IsReadable}], IsMutable[{IsMutable}], Attributes[{Attributes.Length}]")]
    public class PropertyModel
    {
        public AttributeModel[] Attributes { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsReadable { get; set; }
        public bool IsMutable { get; set; }

        public PropertyModel(
            AttributeModel[] attributes,
            string type,
            string name,
            bool isReadable,
            bool isMutable
        )
        {
            Attributes = attributes;
            Type = type;
            Name = name.EscapeKeyword();
            IsReadable = isReadable;
            IsMutable = isMutable;
        }
    }
}
