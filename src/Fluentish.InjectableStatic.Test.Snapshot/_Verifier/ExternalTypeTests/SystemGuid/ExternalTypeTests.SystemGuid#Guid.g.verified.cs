﻿//HintName: Guid.g.cs
// <auto-generated />
#nullable enable
#pragma warning disable
namespace Fluentish.Injectable.System
{
    /// <inheritdoc cref="global::System.Guid"/>
    [global::System.Diagnostics.DebuggerStepThrough]
    public class GuidService: IGuid
    {
        /// <inheritdoc cref="global::System.Guid.Empty"/>
        public global::System.Guid Empty
        {
            get => global::System.Guid.Empty;
        }

        /// <inheritdoc cref="global::System.Guid.Parse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Guid Parse(string input) => global::System.Guid.Parse(input);

        /// <inheritdoc cref="global::System.Guid.Parse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Guid Parse(global::System.ReadOnlySpan<char> input) => global::System.Guid.Parse(input);

        /// <inheritdoc cref="global::System.Guid.TryParse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryParse([global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] string? input, out global::System.Guid result) => global::System.Guid.TryParse(input, out result);

        /// <inheritdoc cref="global::System.Guid.TryParse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryParse(global::System.ReadOnlySpan<char> input, out global::System.Guid result) => global::System.Guid.TryParse(input, out result);

        /// <inheritdoc cref="global::System.Guid.ParseExact"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Guid ParseExact(string input, [global::System.Diagnostics.CodeAnalysis.StringSyntaxAttribute("GuidFormat")] string format) => global::System.Guid.ParseExact(input, format);

        /// <inheritdoc cref="global::System.Guid.ParseExact"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Guid ParseExact(global::System.ReadOnlySpan<char> input, [global::System.Diagnostics.CodeAnalysis.StringSyntaxAttribute("GuidFormat")] global::System.ReadOnlySpan<char> format) => global::System.Guid.ParseExact(input, format);

        /// <inheritdoc cref="global::System.Guid.TryParseExact"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryParseExact([global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] string? input, [global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true), global::System.Diagnostics.CodeAnalysis.StringSyntaxAttribute("GuidFormat")] string? format, out global::System.Guid result) => global::System.Guid.TryParseExact(input, format, out result);

        /// <inheritdoc cref="global::System.Guid.TryParseExact"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryParseExact(global::System.ReadOnlySpan<char> input, [global::System.Diagnostics.CodeAnalysis.StringSyntaxAttribute("GuidFormat")] global::System.ReadOnlySpan<char> format, out global::System.Guid result) => global::System.Guid.TryParseExact(input, format, out result);

        /// <inheritdoc cref="global::System.Guid.Parse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Guid Parse(string s, global::System.IFormatProvider? provider) => global::System.Guid.Parse(s, provider);

        /// <inheritdoc cref="global::System.Guid.TryParse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryParse([global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] string? s, global::System.IFormatProvider? provider, out global::System.Guid result) => global::System.Guid.TryParse(s, provider, out result);

        /// <inheritdoc cref="global::System.Guid.Parse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Guid Parse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider) => global::System.Guid.Parse(s, provider);

        /// <inheritdoc cref="global::System.Guid.TryParse"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryParse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider, out global::System.Guid result) => global::System.Guid.TryParse(s, provider, out result);

        /// <inheritdoc cref="global::System.Guid.NewGuid"/>
        [global::System.Diagnostics.DebuggerStepThrough]
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public global::System.Guid NewGuid() => global::System.Guid.NewGuid();

    }
}
#pragma warning restore
