﻿//HintName: FunctionPointer.g.cs
// <auto-generated />
#pragma warning disable
namespace Fluentish.Injectable.MethodReturnTests
{
    /// <inheritdoc cref="global::MethodReturnTests.FunctionPointer"/>
    [global::System.Diagnostics.DebuggerStepThrough]
    public unsafe class FunctionPointerService : IFunctionPointer
    {
        /// <inheritdoc cref="global::MethodReturnTests.FunctionPointer.Test"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public delegate*<int, bool> Test()
            => global::MethodReturnTests.FunctionPointer.Test();

    }
}
#pragma warning restore
