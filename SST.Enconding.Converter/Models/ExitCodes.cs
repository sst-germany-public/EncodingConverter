// Copyright© 2025 sst-germany.de. All rights reserved. SST Scheubeck GmbH, Albrecht-Dürer-Str.36, 06217 Merseburg, Germany.

namespace SST.Encoding.Converter.Models
{
    public enum ErrorCodes : int
    {
        Ok = 0,
        FoundProblems = 1,
        CommandLine = 2,

        InputDirectoryNotFound = 3,
        UnexpectedException = 4,
    }
}
