﻿//HintName: EventHandler.g.cs
// <auto-generated />
#pragma warning disable
namespace Fluentish.Injectable.EventTests
{
    /// <inheritdoc cref="global::EventTests.EventHandler"/>
    [global::System.Diagnostics.DebuggerStepThrough]
    public class EventHandlerService: IEventHandler
    {
        /// <inheritdoc cref="global::EventTests.EventHandler.Test"/>
        public event global::System.EventHandler Test
        {
            add => global::EventTests.EventHandler.Test += value;
            remove => global::EventTests.EventHandler.Test -= value;
        }
    }
}
#pragma warning restore
