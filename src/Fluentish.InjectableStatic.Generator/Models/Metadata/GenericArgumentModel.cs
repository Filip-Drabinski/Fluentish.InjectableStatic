using System.Diagnostics;

namespace Fluentish.InjectableStatic.Generator.Models.Metadata
{
    [DebuggerDisplay("{Name,nq}")]
    public class GenericArgumentModel
    {
        public string Name { get; set; }
        public GenericArgumentConstraintsModel? Constraints { get; set; }

        public GenericArgumentModel(
            string name,
            GenericArgumentConstraintsModel? constraints
        )
        {
            Name = name;
            Constraints = constraints;
        }
    }

    public class GenericArgumentConstraintsModel
    {
        public bool HasReferenceTypeConstraint { get; set; }
        public bool HasValueTypeConstraint { get; set; }
        public bool HasUnmanagedTypeConstraint { get; set; }
        public bool HasTypeConstraint { get; set; }
        public string[] TypeConstraints { get; set; }
        public bool HasConstructorConstraint { get; set; }

        public GenericArgumentConstraintsModel(
            bool hasReferenceTypeConstraint,
            bool hasValueTypeConstraint,
            bool hasUnmanagedTypeConstraint,
            bool hasTypeConstraint,
            string[] typeConstraints,
            bool hasConstructorConstraint
        )
        {
            HasReferenceTypeConstraint = hasReferenceTypeConstraint;
            HasValueTypeConstraint = hasValueTypeConstraint;
            HasUnmanagedTypeConstraint = hasUnmanagedTypeConstraint;
            HasTypeConstraint = hasTypeConstraint;
            TypeConstraints = typeConstraints;
            HasConstructorConstraint = hasConstructorConstraint;
        }
    }
}
