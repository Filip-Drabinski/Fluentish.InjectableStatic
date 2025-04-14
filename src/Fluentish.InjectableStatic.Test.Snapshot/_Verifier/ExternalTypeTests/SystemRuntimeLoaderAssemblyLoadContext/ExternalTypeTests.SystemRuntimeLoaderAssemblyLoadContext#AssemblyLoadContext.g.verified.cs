﻿//HintName: AssemblyLoadContext.g.cs
// <auto-generated />
#nullable enable
#pragma warning disable
namespace Fluentish.Injectable.System.Runtime.Loader
{
    /// <inheritdoc cref="global::System.Runtime.Loader.AssemblyLoadContext"/>
    [global::System.Diagnostics.DebuggerStepThrough]
    public class AssemblyLoadContextService : IAssemblyLoadContext
    {
        /// <inheritdoc cref="global::System.Runtime.Loader.AssemblyLoadContext.GetLoadContext"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Runtime.Loader.AssemblyLoadContext? GetLoadContext(global::System.Reflection.Assembly assembly) => global::System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(assembly);

        /// <inheritdoc cref="global::System.Runtime.Loader.AssemblyLoadContext.GetAssemblyName"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Reflection.AssemblyName GetAssemblyName(string assemblyPath) => global::System.Runtime.Loader.AssemblyLoadContext.GetAssemblyName(assemblyPath);

        /// <inheritdoc cref="global::System.Runtime.Loader.AssemblyLoadContext.EnterContextualReflection"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Runtime.Loader.AssemblyLoadContext.ContextualReflectionScope EnterContextualReflection(global::System.Reflection.Assembly? activating) => global::System.Runtime.Loader.AssemblyLoadContext.EnterContextualReflection(activating);

        /// <inheritdoc cref="global::System.Runtime.Loader.AssemblyLoadContext.Default"/>
        public global::System.Runtime.Loader.AssemblyLoadContext Default
        {
            get => global::System.Runtime.Loader.AssemblyLoadContext.Default;
        }

        /// <inheritdoc cref="global::System.Runtime.Loader.AssemblyLoadContext.All"/>
        public global::System.Collections.Generic.IEnumerable<global::System.Runtime.Loader.AssemblyLoadContext> All
        {
            get => global::System.Runtime.Loader.AssemblyLoadContext.All;
        }

        /// <inheritdoc cref="global::System.Runtime.Loader.AssemblyLoadContext.CurrentContextualReflectionContext"/>
        public global::System.Runtime.Loader.AssemblyLoadContext? CurrentContextualReflectionContext
        {
            get => global::System.Runtime.Loader.AssemblyLoadContext.CurrentContextualReflectionContext;
        }

    }
}
#pragma warning restore
