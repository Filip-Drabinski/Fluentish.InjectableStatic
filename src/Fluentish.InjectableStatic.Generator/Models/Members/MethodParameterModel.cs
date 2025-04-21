using Fluentish.InjectableStatic.Generator.Extensions;
using Fluentish.InjectableStatic.Generator.Models.Metadata;
using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models.Members
{
    [DebuggerDisplay("{Type,nq} {Name,nq} | Modifier[{Modifier}], Attributes[{Attributes.Length}]")]
    public class MethodParameterModel
    {
        public AttributeModel[] Attributes { get; set; }
        public string Modifier { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public MethodParameterModel(
            AttributeModel[] attributes,
            string modifier,
            string type,
            string name
        )
        {
            Attributes = attributes;
            Modifier = modifier;
            Type = type;
            Name = name.EscapeKeyword();
        }
    }
}
