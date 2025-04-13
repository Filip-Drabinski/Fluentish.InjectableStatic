//HintName: InjectableNamespacePrefixAttribute.g.cs
namespace Fluentish.InjectableStatic
{

    [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class InjectableNamespacePrefixAttribute : System.Attribute
    {
        public string NamespacePrefix { get; }

        public InjectableNamespacePrefixAttribute(string namespacePrefix)
        {
            NamespacePrefix = namespacePrefix;
        }
    }
}