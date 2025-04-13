using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class NamespacePrefixTests
    {
        private readonly IncrementalGeneratorVerifier<NamespacePrefixTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task NotDefined()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(NamespacePrefixTests.NotDefined))]
                    
                    namespace NamespacePrefixTests
                    {
                        public static class NotDefined
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
        public async Task Empty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.InjectableNamespacePrefix("")]
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(NamespacePrefixTests.Empty))]
                    
                    namespace NamespacePrefixTests
                    {
                        public static class Empty
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
        public async Task WhiteSpace()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.InjectableNamespacePrefix(" ")]
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(NamespacePrefixTests.Empty))]
                    
                    namespace NamespacePrefixTests
                    {
                        public static class Empty
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
        public async Task Custom()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.InjectableNamespacePrefix("CustomPrefix")]
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(NamespacePrefixTests.CustomPrefix))]
                    
                    namespace NamespacePrefixTests
                    {
                        public static class CustomPrefix
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
        public async Task CustomDot()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.InjectableNamespacePrefix("CustomPrefix.")]
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(NamespacePrefixTests.CustomPrefix))]
                    
                    namespace NamespacePrefixTests
                    {
                        public static class CustomPrefix
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
    }
}
