namespace Fluentish.InjectableStatic.Generator
{
    public record InjectableStaticConfiguration
    {
        public string? Namespace { get; set; }
        public string EndLine { get; set; }

        public InjectableStaticConfiguration(
            string? @namespace,
            string endLine
        )
        {
            Namespace = SanitizeNamespace(@namespace);
            EndLine = endLine;
        }


        private static string SanitizeNamespace(string? @namespace)
        {
            if (@namespace is null)
            {
                return "Fluentish.Injectable.";
            }

            if (string.IsNullOrWhiteSpace(@namespace))
            {
                return "";
            }

            if (!@namespace.EndsWith("."))
            {
                return @namespace + ".";
            }

            return @namespace;
        }
    }
}