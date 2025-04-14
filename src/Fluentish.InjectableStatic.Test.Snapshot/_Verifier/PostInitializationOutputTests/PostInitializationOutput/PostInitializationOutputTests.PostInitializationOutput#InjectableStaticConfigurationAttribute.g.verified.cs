//HintName: InjectableStaticConfigurationAttribute.g.cs
namespace Fluentish.InjectableStatic
{
    public enum NamespaceMode
    {
        Prefix = 0,
        Const = 1,
    }

    [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class InjectableStaticConfigurationAttribute : System.Attribute
    {
        public NamespaceMode NamespaceMode { get; set; }
        public string Namespace { get; set; }
    }
}