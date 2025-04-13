﻿//HintName: InjectableStaticAttribute.g.cs
namespace Fluentish.InjectableStatic
{
    public enum FilterType
    {
        Exclude = 0,
        Include = 1
    }

    [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class InjectableAttribute : System.Attribute
    {
        public System.Type TargetType { get; }
        public Fluentish.InjectableStatic.FilterType FilterType { get; }
        public string[] FilteredMembers { get; }

        public InjectableAttribute(System.Type targetType)
        {
            TargetType = targetType;
            FilterType = Fluentish.InjectableStatic.FilterType.Exclude;
            FilteredMembers = System.Array.Empty<string>();
        }

        public InjectableAttribute(
            System.Type targetType,
            Fluentish.InjectableStatic.FilterType filterType,
            params string[] filteredMembers
        )
        {
            TargetType = targetType;
            FilterType = filterType;
            FilteredMembers = filteredMembers;
        }
    }
}