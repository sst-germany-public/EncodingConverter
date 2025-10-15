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
        [CommandLine.Option('d', "directory", Required = false, HelpText = "")]
        public string? Directory { get; set; } = ".\\";

        [CommandLine.Option('s', "searchpattern", Required = false, HelpText = "Sample: '*.h' or '*.h,*.cpp'.")]
        public string? SearchPattern { get; set; } = "*.*";

        [CommandLine.Option('r', "recursive", Required = false, HelpText = "")]
        public bool Recursive { get; set; } = false;

        [CommandLine.Option("InputCodePage", Required = false, HelpText = "")]
        public int InputCodePage { get; set; } = 1252;

        [CommandLine.Option("OutputCodePage", Required = false, HelpText = "")]
        public int OutputCodePage { get; set; } = 65001;


        [CommandLine.Option("verbose", Required = false, HelpText = "")]
        public bool Verbose { get; set; } = false;
    }
}
