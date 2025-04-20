using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class GenericTypeTests
    {
        private readonly IncrementalGeneratorVerifier<GenericTypeTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task GenericType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericTypeTests.GenericType<>))]
                    
                    namespace GenericTypeTests
                    {
                        public static class GenericType<TType>
                        {
                            public static readonly TType Test = default!;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task GenericTypeConstraintReferenceType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericTypeTests.GenericTypeConstraintReferenceType<>))]
                    
                    namespace GenericTypeTests
                    {
                        public static class GenericTypeConstraintReferenceType<TType> where TType: class
                        {
                            public static readonly TType Test = default!;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task GenericTypeMultiConstraint()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericTypeTests.GenericTypeConstraintReferenceType<>))]
                    
                    namespace GenericTypeTests
                    {
                        public static class GenericTypeConstraintReferenceType<TType> where TType: class, System.IDisposable, new()
                        {
                            public static readonly TType Test = default!;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task GenericTypeConstraintValueType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericTypeTests.GenericTypeConstraintValueType<>))]
                    
                    namespace GenericTypeTests
                    {
                        public static class GenericTypeConstraintValueType<TType> where TType: struct
                        {
                            public static readonly TType Test = default!;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task GenericTypeConstraintType()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.IDisposable).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericTypeTests.GenericTypeConstraintType<>))]
                    
                    namespace GenericTypeTests
                    {
                        public static class GenericTypeConstraintType<TType> where TType: System.IDisposable
                        {
                            public static readonly TType Test = default!;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
    }
}