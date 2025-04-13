using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class MethodReturnTests
    {
        private readonly IncrementalGeneratorVerifier<MethodReturnTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task Void()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.Void))]
                
                    namespace MethodReturnTests
                    {
                        public static class Void
                        {
                            public static void Test()
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
        public async Task Primitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.Primitive))]
                
                    namespace MethodReturnTests
                    {
                        public static class Primitive
                        {
                            public static int Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ReferenceType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.ReferenceType))]
                
                    namespace MethodReturnTests
                    {
                        public class Example 
                        {
                        }

                        public static class ReferenceType
                        {
                            public static Example Test() => default!;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ValueType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.ValueType))]
                
                    namespace MethodReturnTests
                    {
                        public struct Example 
                        {
                        }

                        public static class ValueType
                        {
                            public static Example Test() => default!;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableReferenceType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    #nullable enable
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.NullableReferenceType))]
                    
                    namespace MethodReturnTests
                    {
                        public class Example 
                        {
                        }
                    
                        public static class NullableReferenceType
                        {
                            public static Example? Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task NullableValueType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.NullableValueType))]
                    
                    namespace MethodReturnTests
                    {
                        public struct Example 
                        {
                        }

                        public static class NullableValueType
                        {
                            public static Example? Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task NullableDateTime()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.NullableDateTime))]
                    
                    namespace MethodReturnTests
                    {
                        public static class NullableDateTime
                        {
                            public static System.DateTime? Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task StructNullable()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    #nullable enable
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.StructNullable))]
                    
                    namespace MethodReturnTests
                    {
                        public struct Example 
                        {
                        }
                    
                        public static class StructNullable
                        {
                            public static System.Nullable<Example> Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task StructNullableValueType()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.NullableValueType))]
                    
                    namespace MethodReturnTests
                    {
                        public struct Example 
                        {
                        }

                        public static class NullableValueType
                        {
                            public static System.Nullable<Example> Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task TuplePrimive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.Tuple))]
                    
                    namespace MethodReturnTests
                    {
                        public static class Tuple
                        {
                            public static (string left, object right) Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task Tuple()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.Tuple))]
                    
                    namespace MethodReturnTests
                    {
                        public class Example
                        {
                        }

                        public static class Tuple
                        {
                            public static (Example left, Example right) Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task TupleClassPrimitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.TupleClassPrimitive))]
                    
                    namespace MethodReturnTests
                    {
                        public class Example
                        {
                        }

                        public static class TupleClassPrimitive
                        {
                            public static System.Tuple<Example, Example> Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }
        [Fact]
        public async Task TupleClass()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.TupleClass))]
                    
                    namespace MethodReturnTests
                    {
                        public static class TupleClass
                        {
                            public static System.Tuple<string, object> Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task ArrayPrimitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.PrimitiveArray))]
                    
                    namespace MethodReturnTests
                    {
                        public static class PrimitiveArray
                        {
                            public static object[] Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task Array()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.Array))]
                    
                    namespace MethodReturnTests
                    {
                        public class Example
                        {
                        }

                        public static class Array
                        {
                            public static Example[] Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task Pointer()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.Pointer))]
                    
                    namespace MethodReturnTests
                    {
                        public static unsafe class Pointer
                        {
                            public static int* Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput(),
                configureCompilation: options => options.WithAllowUnsafe(true)
            );

            res.Assert();
        }


        [Fact]
        public async Task Dynamic()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.Dynamic))]
                    
                    namespace MethodReturnTests
                    {
                        public static class Dynamic
                        {
                            public static dynamic Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task FunctionPointer()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.FunctionPointer))]
                    
                    namespace MethodReturnTests
                    {
                        public static unsafe class FunctionPointer
                        {
                            public static delegate*<int, bool> Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput(),
                configureCompilation: options => options.WithAllowUnsafe(true)
            );

            res.Assert();
        }

        [Fact]
        public async Task List()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.List))]
                    
                    namespace MethodReturnTests
                    {
                        public static class List
                        {
                            public static System.Collections.Generic.List<string> Test() => default;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput(),
                configureCompilation: options => options.WithAllowUnsafe(true)
            );

            res.Assert();
        }

        [Fact]
        public async Task NestedGeneric()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodReturnTests.NestedGeneric))]

                    namespace MethodReturnTests
                    {
                        public class OuterType
                        {
                            public class InnerType<T>
                            {
                            }
                        }

                        public static class NestedGeneric
                        {
                            public static OuterType.InnerType<System.DateTime> Test() => default;
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