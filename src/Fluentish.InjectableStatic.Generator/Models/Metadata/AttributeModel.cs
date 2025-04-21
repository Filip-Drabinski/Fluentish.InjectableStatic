using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models.Metadata
{
    [DebuggerDisplay("{TypeFullName,nq} | Arguments[{Arguments.Length}], NamedArguments[{NamedArguments.Length}]")]
    public class AttributeModel
    {
        public string TypeFullName { get; set; }
        public string[] Arguments { get; set; }
        public (string name, string value)[] NamedArguments { get; set; }

        public AttributeModel(
            string typeFullName,
            string[] arguments,
            (string name, string value)[] namedArguments
        )
        {
            TypeFullName = typeFullName;
            Arguments = arguments;
            NamedArguments = namedArguments;
        }
    }
}
