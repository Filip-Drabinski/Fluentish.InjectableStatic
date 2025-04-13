using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class PropertyAccessibilityTests
    {
        private readonly IncrementalGeneratorVerifier<PropertyAccessibilityTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task PublicProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessibilityTests.PublicProperty))]
                
                    namespace PropertyAccessibilityTests
                    {
                        public static class PublicProperty
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
        public async Task InternalProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessibilityTests.InternalProperty))]
                
                    namespace PropertyAccessibilityTests
                    {
                        public static class InternalProperty
                        {
                            internal static int Test { get; set; }
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task PrivateProperty()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(PropertyAccessibilityTests.PrivateProperty))]
                
                    namespace PropertyAccessibilityTests
                    {
                        public static class PrivateProperty
                        {
                            private static int Test { get; set; }
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