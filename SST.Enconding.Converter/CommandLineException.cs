// Copyright© 2025 sst-germany.de. All rights reserved. SST Scheubeck GmbH, Albrecht-Dürer-Str.36, 06217 Merseburg, Germany.
using System;

namespace SST.Encoding.Converter
{
    [Serializable]
    public class CommandLineException : Exception
    {
        public Models.ErrorCodes ErrorCode { get; }
        public int ExitCode => (int)ErrorCode;


        public CommandLineException(Models.ErrorCodes code)
        {
            ErrorCode = code;
        }
        public CommandLineException(Models.ErrorCodes code, string message) : base(message)
        {
            ErrorCode = code;
        }
        public CommandLineException(Models.ErrorCodes code, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = code;
        }

        public override string ToString() => $"{nameof(CommandLineException)}: {nameof(CommandLineException.ErrorCode)}({ErrorCode}) {nameof(CommandLineException.ExitCode)}({ExitCode})";
    }
}
