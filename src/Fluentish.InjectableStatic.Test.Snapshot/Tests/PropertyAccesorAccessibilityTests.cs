using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class PropertyAccessorAccessibilityTests
    {
        private readonly IncrementalGeneratorVerifier<PropertyAccessorAccessibilityTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task PublicGetPublicSetProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessorAccessibilityTests.PublicGetPrivateSetProperty))]
                
                    namespace PropertyAccessorAccessibilityTests
                    {
                        public static class PublicGetPrivateSetProperty
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
        public async Task PublicGetPrivateSetProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessorAccessibilityTests.PublicGetPrivateSetProperty))]
                
                    namespace PropertyAccessorAccessibilityTests
                    {
                        public static class PublicGetPrivateSetProperty
                        {
                            public static int Test { get; private set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task PublicGetInternalSetProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessorAccessibilityTests.PublicGetPrivateSetProperty))]
                
                    namespace PropertyAccessorAccessibilityTests
                    {
                        public static class PublicGetPrivateSetProperty
                        {
                            public static int Test { get; internal set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task PublicSetPublicGetProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessorAccessibilityTests.PublicGetPrivateSetProperty))]
                
                    namespace PropertyAccessorAccessibilityTests
                    {
                        public static class PublicGetPrivateSetProperty
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
        public async Task PublicSetPrivateGetProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessorAccessibilityTests.PublicGetPrivateSetProperty))]
                
                    namespace PropertyAccessorAccessibilityTests
                    {
                        public static class PublicGetPrivateSetProperty
                        {
                            public static int Test { private get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task PublicSetInternalGetProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessorAccessibilityTests.PublicGetPrivateSetProperty))]
                
                    namespace PropertyAccessorAccessibilityTests
                    {
                        public static class PublicGetPrivateSetProperty
                        {
                            public static int Test { internal get; set; }
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