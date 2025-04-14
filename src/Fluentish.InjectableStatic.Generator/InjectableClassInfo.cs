using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Generator
{
    public enum FilterType
    {
        Exclude = 0,
        Include = 1
    }

    public class InjectableClassInfo
    {
        /// <summary>
        /// required to avoid nullablility issues after using `Where` with `is not null` filtering
        /// </summary>
        public static readonly InjectableClassInfo Default = new(null!, FilterType.Exclude, System.Array.Empty<string>());

        public INamedTypeSymbol type;
        public FilterType filter;
        public string[] members;

        public InjectableClassInfo(
            INamedTypeSymbol typeSymbol,
            FilterType filterType,
            string[] filteredMembers
        )
        {
            type = typeSymbol;
            filter = filterType;
            members = filteredMembers;
        }
    }
}
