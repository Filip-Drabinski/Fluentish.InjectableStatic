using System.Runtime.CompilerServices;
using VerifyTests;

namespace Fluentish.InjectableStatic.Test.Snapshot.Setup
{
    internal class VerifySourceGeneratorsSetup
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifySourceGenerators.Initialize();
        }
    }
}

