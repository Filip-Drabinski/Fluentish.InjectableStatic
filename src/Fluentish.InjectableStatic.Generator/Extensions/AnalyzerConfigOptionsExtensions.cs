using Microsoft.CodeAnalysis.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Extensions
{
    internal static class AnalyzerConfigOptionsExtensions
    {
        public static string GetNewLineSymbol(this AnalyzerConfigOptions options)
        {
            if (!options.TryGetValue("end_of_line", out var value))
            {
                return "\r\n";
            }

            return value switch
            {
                "lf" => "\n",
                "crlf" => "\r\n",
                "cr" => "\r",
                _ => "\r\n"
            };
        }
    }
}