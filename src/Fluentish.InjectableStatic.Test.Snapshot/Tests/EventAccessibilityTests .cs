using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class EventAccessibilityTests
    {
        private readonly IncrementalGeneratorVerifier<EventAccessibilityTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task PublicEvent()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [
                    "CS0067" // The event '???' is never used
                ],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(EventAccessibilityTests.PublicEvent))]
                
                    namespace EventAccessibilityTests
                    {
                        public static class PublicEvent
                        {
                            public static event System.EventHandler Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task InternalEvent()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [
                    "CS0067" // The event '???' is never used
                ],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(EventAccessibilityTests.InternalEvent))]
                
                    namespace EventAccessibilityTests
                    {
                        public static class InternalEvent
                        {
                            internal static event System.EventHandler Test;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task PrivateEvent()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [
                    "CS0067" // The event '???' is never used
                ],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(EventAccessibilityTests.PrivateEvent))]
                
                    namespace EventAccessibilityTests
                    {
                        public static class PrivateEvent
                        {
                            private static event System.EventHandler Test;
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