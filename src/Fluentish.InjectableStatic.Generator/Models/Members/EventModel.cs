using Fluentish.InjectableStatic.Generator.Models.Metadata;
using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models.Members
{
    [DebuggerDisplay("{Type,nq} {Name,nq} | IsAddable[{IsAddable}], IsRemovable[{IsRemovable}], Attributes[{Attributes.Length}]")]
    public class EventModel
    {
        public AttributeModel[] Attributes { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsAddable { get; set; }
        public bool IsRemovable { get; set; }

        public EventModel(
            AttributeModel[] attributes,
            string type,
            string name,
            bool isAddable,
            bool isRemovable
        )
        {
            Attributes = attributes;
            Type = type;
            Name = name;
            IsAddable = isAddable;
            IsRemovable = isRemovable;
        }
    }
}
