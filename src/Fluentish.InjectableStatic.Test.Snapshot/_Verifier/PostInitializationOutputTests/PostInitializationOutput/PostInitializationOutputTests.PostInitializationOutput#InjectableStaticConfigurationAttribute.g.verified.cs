//HintName: InjectableStaticConfigurationAttribute.g.cs
namespace Fluentish.InjectableStatic
{

    [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class InjectableStaticConfigurationAttribute : System.Attribute
    {
        public string NamespacePrefix { get; }

        public InjectableStaticConfigurationAttribute(string namespacePrefix)
        {
            NamespacePrefix = namespacePrefix;
        }
    }
}