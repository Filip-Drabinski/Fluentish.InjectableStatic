using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Generator.GeneratedAttributes
{
    public enum FilterType
    {
        Exclude = 0,
        Include = 1
    }

    public class InjectableStaticInfo
    {
        /// <summary>
        /// required to avoid nullablility issues after using `Where` with `is not null` filtering
        /// </summary>
        public static readonly InjectableStaticInfo Default = new(null!, FilterType.Exclude, []);

        public INamedTypeSymbol type;
        public FilterType filter;
        public string[] members;

        public InjectableStaticInfo(
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
