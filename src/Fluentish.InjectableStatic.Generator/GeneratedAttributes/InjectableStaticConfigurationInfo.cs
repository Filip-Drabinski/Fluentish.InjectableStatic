namespace Fluentish.InjectableStatic.Generator.GeneratedAttributes
{
    public enum NamespaceMode
    {
        Prefix = 0,
        Const = 1,
    }

    public record InjectableStaticConfigurationInfo
    {
        public string Namespace { get; set; }
        public string EndLine { get; set; }
        public NamespaceMode NamespaceMode { get; }

        public InjectableStaticConfigurationInfo(
            string endLine,
            NamespaceMode namespaceMode,
            string? @namespace
        )
        {
            EndLine = endLine;
            NamespaceMode = namespaceMode;
            Namespace = SanitizeNamespace(@namespace, namespaceMode);
        }


        private static string SanitizeNamespace(string? @namespace, NamespaceMode namespaceMode)
        {
            if (@namespace is null && namespaceMode == NamespaceMode.Prefix)
            {
                return "Fluentish.Injectable.";
            }

            if (@namespace is null && namespaceMode == NamespaceMode.Const)
            {
                return "Fluentish.Injectable";
            }

            if (string.IsNullOrWhiteSpace(@namespace))
            {
                return "";
            }

            if (!@namespace!.EndsWith(".") && namespaceMode == NamespaceMode.Prefix)
            {
                return @namespace + ".";
            }
            if (@namespace!.EndsWith(".") && namespaceMode == NamespaceMode.Const)
            {
                return @namespace.Substring(0, @namespace.Length - 1);
            }

            return @namespace;
        }
    }
}