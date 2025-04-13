using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class MethodParameterTests
    {
        private readonly IncrementalGeneratorVerifier<MethodParameterTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task ParameterPrimitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodParameterTests.ParameterPrimitive))]
                
                    namespace MethodParameterTests
                    {
                        public static class ParameterPrimitive
                        {
                            public static void Test(int parameter)
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
        public async Task MultiParameterPrimitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodParameterTests.MultiParameterPrimitive))]
                
                    namespace MethodParameterTests
                    {
                        public static class MultiParameterPrimitive
                        {
                            public static void Test(int parameter0, int parameter1)
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
        public async Task ParameterOutPrimitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodParameterTests.ParameterOutPrimitive))]
                
                    namespace MethodParameterTests
                    {
                        public static class ParameterOutPrimitive
                        {
                            public static void Test(out int parameter)
                            {
                                parameter = default;
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
        public async Task ParameterInPrimitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodParameterTests.ParameterInPrimitive))]
                
                    namespace MethodParameterTests
                    {
                        public static class ParameterInPrimitive
                        {
                            public static void Test(in int parameter)
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
        public async Task ParameterParamsPrimitive()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MethodParameterTests.ParameterParamsPrimitive))]
                
                    namespace MethodParameterTests
                    {
                        public static class ParameterParamsPrimitive
                        {
                            public static void Test(params int[] parameter)
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