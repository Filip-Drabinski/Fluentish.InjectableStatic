using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class FieldReturnTypeTests
    {
        private readonly IncrementalGeneratorVerifier<FieldReturnTypeTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task PrimitiveField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnPrimitive))]
                
                    namespace FieldReturnTypeTests
                    {
                        public static class ReturnPrimitive
                        {
                            public static int Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ReferenceTypeField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnReferenceType))]
                
                    namespace FieldReturnTypeTests
                    {
                        public class Example 
                        {
                        }

                        public static class ReturnReferenceType
                        {
                            public static Example Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ValueTypeField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnValueType))]
                
                    namespace FieldReturnTypeTests
                    {
                        public struct Example 
                        {
                        }

                        public static class ReturnValueType
                        {
                            public static Example Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableReferenceTypeField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    #nullable enable
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnNullableReferenceType))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public class Example 
                        {
                        }
                    
                        public static class ReturnNullableReferenceType
                        {
                            public static Example? Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableValueTypeField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnNullableValueType))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public struct Example 
                        {
                        }

                        public static class ReturnNullableValueType
                        {
                            public static Example? Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task SugarTuplePrimiveField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnSugarTuple))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public static class ReturnSugarTuple
                        {
                            public static (string left, object right) Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task SugarTupleField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnSugarTuple))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnSugarTuple
                        {
                            public static (Example left, Example right) Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task TuplePrimitiveField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnTuple))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnTuple
                        {
                            public static System.Tuple<Example, Example> Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task TupleField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnTuple))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public static class ReturnTuple
                        {
                            public static System.Tuple<string, object> Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ArrayPrimitiveField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnPrimitiveArray))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public static class ReturnPrimitiveArray
                        {
                            public static object[] Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ArrayField()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(FieldReturnTypeTests.ReturnArray))]
                    
                    namespace FieldReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnArray
                        {
                            public static Example[] Test;
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