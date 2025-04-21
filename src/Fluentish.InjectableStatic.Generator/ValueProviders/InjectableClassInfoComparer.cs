using Fluentish.InjectableStatic.Generator.GeneratedAttributes;
using System;
using System.Collections.Generic;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    public class InjectableClassInfoComparer : IEqualityComparer<InjectableStaticInfo>
    {
        public static readonly InjectableClassInfoComparer Instance = new();

        public bool Equals(InjectableStaticInfo x, InjectableStaticInfo y)
        {
            return Instance.GetHashCode(x) == Instance.GetHashCode(y);
        }

        public int GetHashCode(InjectableStaticInfo obj)
        {
            var hashCodeBuilder = new HashCode();
            hashCodeBuilder.Add(obj.type);
            hashCodeBuilder.Add(obj.filter);
            for (int i = 0; i < obj.members.Length; i++)
            {
                hashCodeBuilder.Add(obj.members[i]);
            }
            return hashCodeBuilder.ToHashCode();
        }
    }
}
