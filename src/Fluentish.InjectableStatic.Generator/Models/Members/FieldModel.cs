using Fluentish.InjectableStatic.Generator.Models.Metadata;
using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models.Members
{
    [DebuggerDisplay("{Type,nq} {Name,nq} | IsMutable[{IsMutable}], Attributes[{Attributes.Length}]")]
    public class FieldModel
    {
        public AttributeModel[] Attributes { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsMutable { get; set; }

        public FieldModel(
            AttributeModel[] attributes,
            string type,
            string name,
            bool isMutable
        )
        {
            Attributes = attributes;
            Type = type;
            Name = name;
            IsMutable = isMutable;
        }
    }
}
