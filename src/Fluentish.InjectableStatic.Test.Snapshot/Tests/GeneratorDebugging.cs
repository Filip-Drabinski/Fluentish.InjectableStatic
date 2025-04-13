using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class GeneratorDebugging
    {
        private readonly IncrementalGeneratorVerifier<GeneratorDebugging, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task Playground()
        {
            var res = await _verifier.Verify(
                testName: nameof(Playground),
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """

                    """
                ]
            );
            res.Assert();
        }
    }
}