// Copyright© 2025 sst-germany.de. All rights reserved. SST Scheubeck GmbH, Albrecht-Dürer-Str.36, 06217 Merseburg, Germany.

namespace EncodingConverter.Models
{
    /// <summary>
    /// Stellt die verfügbaren Optionen für die Kommandozeile dar.
    /// </summary>
    public class CommandLineOptions
    {
        /// <summary>
        /// Pfad zum Verzeichnis das geprüft werden soll.
        /// </summary>
        [CommandLine.Option('d', "directory", Required = false, HelpText = "Specifies the directory that is used to search for the files.")]
        public string? Directory { get; set; } = ".\\";

        [CommandLine.Option('s', "searchpattern", Required = false, HelpText = "Sample: '*.h' or '*.h,*.cpp'.")]
        public string? SearchPattern { get; set; } = "*.*";

        [CommandLine.Option('r', "recursive", Required = false, HelpText = "Recursive directory processing.")]
        public bool Recursive { get; set; } = false;

        [CommandLine.Option("InputCodePage", Required = false, HelpText = "Input Codepage that is used for reading the file.")]
        public int InputCodePage { get; set; } = 1252;

        [CommandLine.Option("OutputCodePage", Required = false, HelpText = "Output Codepage that is used to write the file (convert the file to).")]
        public int OutputCodePage { get; set; } = 65001;


        [CommandLine.Option("verbose", Required = false, HelpText = "More verbose console messages.")]
        public bool Verbose { get; set; } = false;
    }
}
