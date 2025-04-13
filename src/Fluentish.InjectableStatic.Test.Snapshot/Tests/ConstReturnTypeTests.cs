using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class ConstReturnTypeTests
    {
        private readonly IncrementalGeneratorVerifier<ConstReturnTypeTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task PrimitiveConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnPrimitive))]
                
                    namespace ConstReturnTypeTests
                    {
                        public static class ReturnPrimitive
                        {
                            public const int Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ReferenceTypeConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnReferenceType))]
                
                    namespace ConstReturnTypeTests
                    {
                        public class Example 
                        {
                        }

                        public static class ReturnReferenceType
                        {
                            public const Example Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ValueTypeConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnValueType))]
                
                    namespace ConstReturnTypeTests
                    {
                        public struct Example 
                        {
                        }

                        public static class ReturnValueType
                        {
                            public static readonly Example Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableReferenceTypeConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    #nullable enable
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnNullableReferenceType))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public class Example 
                        {
                        }
                    
                        public static class ReturnNullableReferenceType
                        {
                            public const Example? Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableValueTypeConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnNullableValueType))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public struct Example 
                        {
                        }

                        public static class ReturnNullableValueType
                        {
                            public static readonly Example? Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task SugarTuplePrimiveConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnSugarTuple))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public static class ReturnSugarTuple
                        {
                            public static readonly (string left, object right) Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task SugarTupleConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnSugarTuple))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnSugarTuple
                        {
                            public static readonly (Example left, Example right) Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task TuplePrimitiveConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnTuple))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnTuple
                        {
                            public const System.Tuple<Example, Example> Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task TupleConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnTuple))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public static class ReturnTuple
                        {
                            public const System.Tuple<string, object> Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ArrayPrimitiveConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnPrimitiveArray))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public static class ReturnPrimitiveArray
                        {
                            public const object[] Test = default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ArrayConst()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(ConstReturnTypeTests.ReturnArray))]
                    
                    namespace ConstReturnTypeTests
                    {
                        public class Example
                        {
                        }

                        public static class ReturnArray
                        {
                            public const Example[] Test = default;
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