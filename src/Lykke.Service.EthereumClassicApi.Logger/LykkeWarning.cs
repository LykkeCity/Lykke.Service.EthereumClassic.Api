﻿using System;
using Lykke.Logs;

namespace Lykke.Service.EthereumClassicApi.Logger
{
    /// <summary>
    ///     This class represents an Warning log event.
    /// </summary>
    public sealed class LykkeWarning : LykkeLogEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LykkeWarning" /> class.
        /// </summary>
        /// <param name="logSource">
        ///     The source that generated the event.
        /// </param>
        /// <param name="logClass">
        ///     The type that generated the event.
        /// </param>
        /// <param name="message">
        ///     The message that is being logged.
        /// </param>
        /// <param name="duration">
        ///     The duration of operation.
        /// </param>
        /// <param name="process">
        ///     The method where the event occured.
        /// </param>
        /// <param name="trigger">
        ///     The message that triggered the log event.
        /// </param>
        /// <param name="cause">
        ///     The exception that caused the log event.
        /// </param>
        /// <inheritdoc />
        public LykkeWarning(string logSource, Type logClass, string message, long? duration, string process,
            object trigger, Exception cause)
        {
            Cause = cause;
            Duration = duration;
            LogSource = logSource;
            LogClass = logClass;
            Message = message;
            Process = process;
            Trigger = trigger;
        }

        /// <summary>
        ///     The exception that caused the log event.
        /// </summary>
        public Exception Cause { get; }

        /// <inheritdoc />
        public override LogLevel LogLevel { get; }
            = LogLevel.Warning;
    }
}
