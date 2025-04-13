using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class PropertyReturnTypeTests
    {
        private readonly IncrementalGeneratorVerifier<PropertyReturnTypeTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task PrimitiveProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnPrimitive))]
                
                    namespace PropertyReturnTypeTests
                    {
                        public static class ReturnPrimitive
                        {
                            public static int Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ReferenceTypeProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnReferenceType))]
                
                    namespace PropertyReturnTypeTests
                    {
                        public class Example 
                        {
                        }

                        public static class ReturnReferenceType
                        {
                            public static Example Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ValueTypeProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnValueType))]
                
                    namespace PropertyReturnTypeTests
                    {
                        public struct Example 
                        {
                        }

                        public static class ReturnValueType
                        {
                            public static Example Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableReferenceTypeProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    #nullable enable
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnNullableReferenceType))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public class Example 
                        {
                        }
                    
                        public static class ReturnNullableReferenceType
                        {
                            public static Example? Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableValueTypeProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnNullableValueType))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public struct Example 
                        {
                        }

                        public static class ReturnNullableValueType
                        {
                            public static Example? Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task SugarTuplePrimiveProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnSugarTuple))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public static class ReturnSugarTuple
                        {
                            public static (string left, object right) Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task SugarTupleProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnSugarTuple))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnSugarTuple
                        {
                            public static (Example left, Example right) Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task TuplePrimitiveProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnTuple))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnTuple
                        {
                            public static System.Tuple<Example, Example> Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task TupleProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnTuple))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public static class ReturnTuple
                        {
                            public static System.Tuple<string, object> Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ArrayPrimitiveProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnPrimitiveArray))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public static class ReturnPrimitiveArray
                        {
                            public static object[] Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ArrayProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyReturnTypeTests.ReturnArray))]
                    
                    namespace PropertyReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnArray
                        {
                            public static Example[] Test { get; set; }
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