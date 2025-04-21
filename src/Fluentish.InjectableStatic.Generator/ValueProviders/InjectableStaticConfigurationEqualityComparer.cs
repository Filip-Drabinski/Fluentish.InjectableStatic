using Fluentish.InjectableStatic.Generator.GeneratedAttributes;
using System;
using System.Collections.Generic;

namespace Fluentish.InjectableStatic.Generator.ValueProviders
{
    public class InjectableStaticConfigurationEqualityComparer : IEqualityComparer<InjectableStaticConfigurationInfo>
    {
        public static readonly InjectableStaticConfigurationEqualityComparer Instance = new();

        public bool Equals(InjectableStaticConfigurationInfo x, InjectableStaticConfigurationInfo y)
        {
            return x.EndLine == y.EndLine
                && x.Namespace == y.Namespace
                && x.NamespaceMode == y.NamespaceMode;
        }

        public int GetHashCode(InjectableStaticConfigurationInfo obj)
        {
            return HashCode.Combine(
                obj.EndLine,
                obj.Namespace,
                obj.NamespaceMode
            );
        }
    }
}
