using System;
using System.Collections.Generic;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    public class InjectableClassInfoComparer : IEqualityComparer<InjectableClassInfo>
    {
        public static readonly InjectableClassInfoComparer Instance = new();

        public bool Equals(InjectableClassInfo x, InjectableClassInfo y)
        {
            return Instance.GetHashCode(x) == Instance.GetHashCode(y);
        }

        public int GetHashCode(InjectableClassInfo obj)
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
