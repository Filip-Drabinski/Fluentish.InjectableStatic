using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class MemberNameKeywordCollisionTests
    {
        private readonly IncrementalGeneratorVerifier<MemberNameKeywordCollisionTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task Event()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [
                    "CS0067" //The * is never used
                ],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MemberNameKeywordCollisionTests.Event))]
                
                    namespace MemberNameKeywordCollisionTests
                    {
                        public static class Event
                        {
                            public static event System.EventHandler @event;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task Field()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MemberNameKeywordCollisionTests.Field))]
                
                    namespace MemberNameKeywordCollisionTests
                    {
                        public static class Field
                        {
                            public static readonly int @event = 0;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task Method()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MemberNameKeywordCollisionTests.Method))]
                
                    namespace MemberNameKeywordCollisionTests
                    {
                        public static class Method
                        {
                            public static int @event() => 0;
                        }
                    }
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );

            res.Assert();
        }

        [Fact]
        public async Task MethodParameter()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MemberNameKeywordCollisionTests.MethodParameter))]
                
                    namespace MemberNameKeywordCollisionTests
                    {
                        public static class MethodParameter
                        {
                            public static void Test(int @event)
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
        public async Task Property()
        {
            var res = await _verifier.Verify(
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(MemberNameKeywordCollisionTests.Property))]
                
                    namespace MemberNameKeywordCollisionTests
                    {
                        public static class Property
                        {
                            public static int @event { get; set; }
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