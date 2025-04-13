using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit.Sdk;

namespace Fluentish.InjectableStatic.Test.Snapshot.Setup
{
    internal class IncrementalGeneratorVerifier<TTestClass, TGenerator> where TGenerator : IIncrementalGenerator, new()
    {
        private readonly TGenerator _generator;
        private readonly string _testClassName;

        public IncrementalGeneratorVerifier()
        {
            _generator = new TGenerator();
            _testClassName = typeof(TTestClass).Name;
        }

        public async Task<IncrementalGeneratorVerifierResult> Verify(
            string[] diagnosticCodesToIgnore,
            string[] sources,
            Func<CSharpParseOptions, CSharpParseOptions>? configureParser = null,
            Func<CSharpCompilationOptions, CSharpCompilationOptions>? configureCompilation = null,
            Action<List<string>>? configureReferenceLocations = null,
            Func<GeneratedSourceResult, bool>? ignoreResult = null,
            [CallerMemberName] string? testName = null
        )
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(testName), "Invalid test name");

            var parserOptions = new CSharpParseOptions();
            var compilationOptions = new CSharpCompilationOptions(default);
            ignoreResult ??= x => false;

            if (configureParser is not null)
            {
                parserOptions = configureParser(parserOptions);
            }
            if (configureCompilation is not null)
            {
                compilationOptions = configureCompilation(compilationOptions);
            }

            var syntaxTrees = sources.Select(x => CSharpSyntaxTree.ParseText(x, options: parserOptions));

            var customReferences = new List<string>();
            configureReferenceLocations?.Invoke(customReferences);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.FullName!.StartsWith("Fluentish.InjectableStatic."))
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                .Select(a => a.Location)
                .Concat(customReferences)
                .Distinct()
                .Select(l => MetadataReference.CreateFromFile(l))
                .ToArray();

            var compilation = CSharpCompilation.Create(
                "UnitTestAssembly",
                syntaxTrees,
                references: references,
                options: compilationOptions
            );

            var generatorDriver = CSharpGeneratorDriver.Create(_generator);
            var driver = generatorDriver.RunGeneratorsAndUpdateCompilation(compilation, out var resultCompilation, out var _);
            var diagnostics = resultCompilation.GetDiagnostics().Where(x => !diagnosticCodesToIgnore.Contains(x.Id)).ToArray();

            var fileChangeMessage = string.Empty;
            try
            {
                var verifierResult = await Verifier.Verify(driver)
                    .UseDirectory($"..\\_Verifier\\{_testClassName}\\{testName}")
                    .IgnoreGeneratedResult(ignoreResult);
            }
            catch (Exception verifyException) when (verifyException.HResult == -2146233088)
            {
                fileChangeMessage = verifyException.Message;
            }

            return new IncrementalGeneratorVerifierResult(fileChangeMessage, diagnostics);
        }
    }

    internal record struct IncrementalGeneratorVerifierResult(string FileChangeMessage, Diagnostic[] Diagnostics)
    {
        public readonly void Assert()
        {
            var errorMessageBuilder = new StringBuilder();
            if (Diagnostics.Length > 0)
            {
                errorMessageBuilder
                    .AppendLine("Compilation problems detected:");

                var maxLength = (
                    severity: "Type".Length,
                    errorCode: "Code".Length,
                    description: "Description".Length,
                    codeFragment: "Snippet".Length,
                    fileInfo: "File".Length,
                    suppressionState: "SuppressionState".Length,
                    details: "Details".Length
                );

                var diagnosticsData = new (string severity, string codeFragment, string description, string fileInfo, string errorCode, string suppressionState, string details)[Diagnostics.Length];
                for (int i = 0; i < Diagnostics.Length; i++)
                {
                    var severity = Diagnostics[i].Severity.ToString();
                    var codeFragment = Diagnostics[i].Location.SourceTree?.GetText().ToString(Diagnostics[i].Location.SourceSpan);
                    var description = Diagnostics[i].GetMessage();
                    var filePath = Diagnostics[i]?.Location?.SourceTree?.FilePath;
                    var line = Diagnostics[i].Location.GetLineSpan().StartLinePosition.Line + 1;
                    var character = Diagnostics[i].Location.GetLineSpan().StartLinePosition.Character + 1;
                    var fileInfo = filePath is null ? null : $"{filePath}({line},{character})";
                    var errorCode = Diagnostics[i].Id;
                    var suppressionState = Diagnostics[i].IsSuppressed ? "Inactive" : "Active";

                    diagnosticsData[i] = (
                        severity ?? "-",
                        codeFragment ?? "-",
                        description ?? "-",
                        fileInfo ?? "-",
                        errorCode ?? "-",
                        suppressionState ?? "-",
                        details: "-"
                    );
                    maxLength.severity = Math.Max(maxLength.severity, diagnosticsData[i].severity.Length);
                    maxLength.codeFragment = Math.Max(maxLength.codeFragment, diagnosticsData[i].codeFragment.Length);
                    maxLength.description = Math.Max(maxLength.description, diagnosticsData[i].description.Length);
                    maxLength.fileInfo = Math.Max(maxLength.fileInfo, diagnosticsData[i].fileInfo.Length);
                    maxLength.errorCode = Math.Max(maxLength.errorCode, diagnosticsData[i].errorCode.Length);
                    maxLength.suppressionState = Math.Max(maxLength.suppressionState, diagnosticsData[i].suppressionState.Length);
                    maxLength.details = Math.Max(maxLength.details, diagnosticsData[i].details.Length);

                }
                var lineFormat = $$"""{0,-{{maxLength.severity}}} | {1,-{{maxLength.errorCode}}} | {2,-{{maxLength.description}}} | {3,-{{maxLength.codeFragment}}} | {4,-{{maxLength.fileInfo}}} | {5,-{{maxLength.suppressionState}}} | {6,-5}""";

                errorMessageBuilder
                    .AppendLine()
                    .AppendFormat(lineFormat, "Type", "Code", "Description", "Snippet", "File", "Suppression State", "Details");

                errorMessageBuilder
                    .AppendLine()
                    .AppendFormat(lineFormat, "----", "----", "-----------", "-------", "----", "----------------", "-------")
                    .AppendLine();
                for (int i = 0; i < diagnosticsData.Length; i++)
                {
                    errorMessageBuilder
                        .AppendFormat(lineFormat, diagnosticsData[i].severity, diagnosticsData[i].errorCode, diagnosticsData[i].description, diagnosticsData[i].codeFragment, diagnosticsData[i].fileInfo, diagnosticsData[i].suppressionState, "-")
                        .AppendLine();
                }

                if (!string.IsNullOrWhiteSpace(FileChangeMessage))
                {
                    errorMessageBuilder
                        .AppendLine()
                        .AppendLine("|--------")
                        .AppendLine();
                }
            }

            if (!string.IsNullOrWhiteSpace(FileChangeMessage))
            {
                errorMessageBuilder
                    .AppendLine("FilesChanged:");

                errorMessageBuilder
                    .AppendLine(FileChangeMessage);
            }

            if (Diagnostics.Length > 0 || !string.IsNullOrWhiteSpace(FileChangeMessage))
            {
                var message = errorMessageBuilder.ToString();
                throw new XunitException(message);
            }
        }
    }
}