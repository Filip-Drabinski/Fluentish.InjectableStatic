using Microsoft.CodeAnalysis;

namespace Fluentish.InjectableStatic.Generator
{
    internal enum FilterType
    {
        Exclude = 0,
        Include = 1
    }

    internal class InjectableClassInfo
    {
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