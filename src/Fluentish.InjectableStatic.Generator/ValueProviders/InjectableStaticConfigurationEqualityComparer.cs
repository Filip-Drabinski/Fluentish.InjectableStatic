using System;
using System.Collections.Generic;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    public class InjectableStaticConfigurationEqualityComparer : IEqualityComparer<InjectableStaticConfiguration>
    {
        public static readonly InjectableStaticConfigurationEqualityComparer Instance = new();

        public bool Equals(InjectableStaticConfiguration x, InjectableStaticConfiguration y)
        {
            return x.EndLine == y.EndLine
                && x.Namespace == y.Namespace;
        }

        public int GetHashCode(InjectableStaticConfiguration obj)
        {
            return HashCode.Combine(
                obj.Namespace,
                obj.EndLine
            );
        }
    }
}
