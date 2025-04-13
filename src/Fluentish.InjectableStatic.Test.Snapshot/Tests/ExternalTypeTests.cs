using Fluentish.InjectableStatic.Generator;
using Fluentish.InjectableStatic.Test.Snapshot.Setup;
using Fluentish.InjectableStatic.Test.Snapshot.Sources;
using System.Threading.Tasks;
using Xunit;

namespace Fluentish.InjectableStatic.Test.Snapshot.Tests
{
    public class ExternalTypeTests
    {
        private readonly IncrementalGeneratorVerifier<ExternalTypeTests, InjectableStaticGenerator> _verifier = new();

        [Fact]
        public async Task SystemConsole()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Console).Assembly.Location);
                    collection.Add(typeof(System.Diagnostics.CodeAnalysis.StringSyntaxAttribute).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Console))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemMath()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Console).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Math))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemConvert()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Convert).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Convert))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemEnvironment()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Environment).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Environment))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemDateTime()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.DateTime).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.DateTime))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemString()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.String).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.String))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemIOFile()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.IO.File).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.IO.File))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemIODirectory()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.IO.Directory).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.IO.Directory))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemIOPath()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.IO.Path).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.IO.Path))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemDiagnosticsDebug()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Diagnostics.Debug).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(
                        typeof(System.Diagnostics.Debug),
                        Fluentish.InjectableStatic.FilterType.Exclude,
                        "SetProvider"
                    )]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }
        [Fact]
        public async Task SystemDiagnosticsTrace()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Diagnostics.Trace).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Diagnostics.Trace))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemThreadingThread()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Threading.Thread).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Threading.Thread))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemThreadingTasksTask()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Threading.Tasks.Task).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Threading.Tasks.Task))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemBitConverter()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.BitConverter).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.BitConverter))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemGuid()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Guid).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Guid))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemNetNetworkInformationNetworkChange()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Net.NetworkInformation.NetworkChange).Assembly.Location);
                    collection.Add(typeof(System.ComponentModel.EditorBrowsableAttribute).Assembly.Location);
                    collection.Add(typeof(System.ObsoleteAttribute).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Net.NetworkInformation.NetworkChange))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }

        [Fact]
        public async Task SystemRuntimeLoaderAssemblyLoadContext()
        {
            var res = await _verifier.Verify(
                configureReferenceLocations: (collection) =>
                {
                    collection.Add(typeof(System.Runtime.Loader.AssemblyLoadContext).Assembly.Location);
                },
                diagnosticCodesToIgnore: [],
                sources: [
                    StaticSource.Program,
                    """
                    [assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Runtime.Loader.AssemblyLoadContext))]
                    """
                ],
                ignoreResult: generatedResult => generatedResult.IsPostInitializationOutput()
            );
            res.Assert();
        }
    }
}
