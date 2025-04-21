using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class GenericMethodTests
    {
        private readonly IncrementalGeneratorVerifier<GenericMethodTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task GenericTypeArgument()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericMethodTests.GenericTypeArgument))]
                    
                    namespace GenericMethodTests
                    {
                        public static class GenericTypeArgument
                        {
                            public static void Test<TType>()
                            {
                            }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task GenericTypeArgumentConstraintReferenceType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericMethodTests.GenericTypeArgumentConstraintReferenceType))]
                    
                    namespace GenericMethodTests
                    {
                        public static class GenericTypeArgumentConstraintReferenceType
                        {
                            public static void Test<TType>() where TType: class
                            {
                            }
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
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericMethodTests.GenericTypeArgumentConstraintReferenceType))]
                    
                    namespace GenericMethodTests
                    {
                        public static class GenericTypeArgumentConstraintReferenceType
                        {
                            public static void Test<TType>() where TType: class, System.IDisposable, new()
                            {
                            }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task GenericTypeArgumentConstraintValueType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericMethodTests.GenericTypeArgumentConstraintValueType))]
                    
                    namespace GenericMethodTests
                    {
                        public static class GenericTypeArgumentConstraintValueType
                        {
                            public static void Test<TType>() where TType: struct
                            {
                            }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task GenericTypeArgumentConstraintType()
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
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(GenericMethodTests.GenericTypeArgumentConstraintType))]
                    
                    namespace GenericMethodTests
                    {
                        public static class GenericTypeArgumentConstraintType
                        {
                            public static void Test<TType>() where TType: System.IDisposable
                            {
                            }
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