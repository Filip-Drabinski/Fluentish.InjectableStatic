using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class PostInitializationOutputTests
    {
        private readonly IncrementalGeneratorVerifier<PostInitializationOutputTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task PostInitializationOutput()
        {
            var res = await _verifier.Verify(
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
