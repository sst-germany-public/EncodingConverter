// Copyright© 2025 sst-germany.de. All rights reserved. SST Scheubeck GmbH, Albrecht-Dürer-Str.36, 06217 Merseburg, Germany.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using EncodingConverter.Models;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace EncodingConverter
{
    internal class Program
    {
        #region nLog instance (s_log)
        private static Logger s_log { get; } = LogManager.GetCurrentClassLogger();
        #endregion

        private static void Main(string[] args)
        {
            // Parse CommandLine
            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed((o) =>
                {
                    // Service Collection erstellen
                    try
                    {
                        #region static void configureColoredConsoleLogging(bool verbose)
                        static void configureColoredConsoleLogging(bool verbose)
                        {
                            // Neue NLog-Konfiguration anlegen
                            var config = new LoggingConfiguration();

                            // ColoredConsole-Target erstellen
                            var consoleTarget = new ColoredConsoleTarget("console")
                            {
                                //Layout = @"${longdate} | ${level:uppercase=true} | ${message} ${exception:format=toString}"
                                Layout = @"${message} ${exception:format=toString}"
                            };

                            // Farben je nach Log-Level festlegen
                            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
                            {
                                Condition = "level == LogLevel.Debug",
                                ForegroundColor = ConsoleOutputColor.Gray
                            });
                            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
                            {
                                Condition = "level == LogLevel.Info",
                                ForegroundColor = ConsoleOutputColor.White
                            });
                            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
                            {
                                Condition = "level == LogLevel.Warn",
                                ForegroundColor = ConsoleOutputColor.Yellow
                            });
                            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
                            {
                                Condition = "level == LogLevel.Error",
                                ForegroundColor = ConsoleOutputColor.Red
                            });
                            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
                            {
                                Condition = "level == LogLevel.Fatal",
                                ForegroundColor = ConsoleOutputColor.White,
                                BackgroundColor = ConsoleOutputColor.Red
                            });

                            // Target mit Log-Level-Regel verbinden
                            config.AddRule(verbose ? LogLevel.Debug : LogLevel.Info, LogLevel.Fatal, consoleTarget);

                            // Konfiguration aktivieren
                            LogManager.Configuration = config;
                        }
                        #endregion
                        configureColoredConsoleLogging(o.Verbose);

                        OnRun(o);
                    }
                    catch (CommandLineException e)
                    {
                        s_log.Error(e, "CommandLineException exception: ");
                        Environment.ExitCode = e.ExitCode;
                    }
                    catch (Exception e)
                    {
                        s_log.Error(e, "Unexpected exception.");
                        Environment.ExitCode = (int)ErrorCodes.UnexpectedException;
                    }
                    finally
                    {
                    }
                })
                .WithNotParsed(onNotParsed);

            #region static void onNotParsed(IEnumerable<CommandLine.Error> errs)
            static void onNotParsed(IEnumerable<CommandLine.Error> errs) => Environment.ExitCode = (int)ErrorCodes.CommandLine;
            #endregion
        }

        private static void OnRun(CommandLineOptions o)
        {
            o.Directory ??= "";
            o.SearchPattern ??= "*.*";

            var searchPatterns = o.SearchPattern.Split(',', StringSplitOptions.RemoveEmptyEntries);

            // Validate Parameter
            var inputDirectory = Path.GetFullPath(o.Directory);
            if (!Directory.Exists(inputDirectory))
            {
                throw new CommandLineException(ErrorCodes.InputDirectoryNotFound);
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var inputEncoding = Encoding.GetEncoding(o.InputCodePage);
            var outputEncoding = Encoding.GetEncoding(o.OutputCodePage);
            //var utf8Bom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);

            s_log.Info("Converting in directory: {0}", inputDirectory);
            s_log.Info("SearchPattern: {0}", o.SearchPattern);
            s_log.Info("InputCodePage: {0} ({1})", inputEncoding.EncodingName, inputEncoding.CodePage);
            s_log.Info("OutputCodePage: {0} ({1})", outputEncoding.EncodingName, outputEncoding.CodePage);

            void convertDirectory(string directory, string[] searchPatterns, bool recursive, out int fileCount, out int errorCount)
            {
                fileCount = 0;
                errorCount = 0;

                var files = searchPatterns.SelectMany(pattern => Directory.GetFiles(directory, pattern));
                foreach (var file in files)
                {
                    try
                    {
                        fileCount++;

                        // Datei komplett einlesen (Encoding automatisch erkennen)
                        string content;
                        using (var reader = new StreamReader(file, inputEncoding, detectEncodingFromByteOrderMarks: true))
                        {
                            content = reader.ReadToEnd();
                        }

                        // In UTF8 mit BOM schreiben
                        File.WriteAllText(file, content, outputEncoding);

                        s_log.Debug("File converted: {0}", file);
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        s_log.Error(ex, "Error converting file: {0}", file);
                    }
                }

                if (recursive)
                {
                    var subDirectories = Directory.GetDirectories(directory);
                    foreach (var subDirectory in subDirectories)
                    {
                        convertDirectory(subDirectory, searchPatterns, recursive, out var fc, out var ec);
                        fileCount += fc;
                        errorCount += ec;
                    }
                }
            }
            convertDirectory(o.Directory, searchPatterns, o.Recursive, out var fileCount, out var errorCount);

            if (errorCount > 0)
            {
                s_log.Error("Conversion of {0} files completed with {1} errors.", fileCount, errorCount);
            }
            else
            {
                s_log.Info("Conversion of {0} files successfully completed.", fileCount);
            }
        }
    }
}