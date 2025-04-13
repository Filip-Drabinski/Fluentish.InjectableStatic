using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class EventTests
    {
        private readonly IncrementalGeneratorVerifier<EventTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task EventHandler()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [
                    "CS0067" //The event '???' is never used
                ],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(EventTests.EventHandler))]
                
                    namespace EventTests
                    {
                        public static class EventHandler
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
    }
}