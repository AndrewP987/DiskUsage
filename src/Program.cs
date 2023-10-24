// ######################################################################
// ### Author: Andrew Photinakis                                      ###
// ### Disk Usage Tool                                                ###
// ######################################################################
using System.Diagnostics;

using Configuration;

// Namespace "DiskUsageTool" contains the application's main class
namespace DiskUsageTool
{
    public class Solution
    {
        // Validate if a directory exists
        static bool ValidateDirectory(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return directoryInfo.Exists;
        }

        // Print a usage message for the application
        static void PrintUsageMessage()
        {
            Console.WriteLine("Usage: du [-s] [-d] [-b] <path>");
            Console.WriteLine("Summarize disk usage of the set of FILES, " +
                "recursively for directories.");
            Console.WriteLine("You MUST specify one of the " +
                "parameters, -s, -d, or -b");
            Console.WriteLine("-s\tRun in single-threaded mode");
            Console.WriteLine("-d\tRun in parallel mode " +
                "(uses all available processors)");
            Console.WriteLine("-b\tRun in both parallel and single-threaded mode.");
            Console.WriteLine("\tRuns parallel mode followed by sequential mode.\n\n");
        }

        static bool IsString(string value)
        {
            // Check if the value is not null and is of type string
            return !string.IsNullOrEmpty(value) && value.GetType() == typeof(string);
        }

        // Analyze command-line arguments and run the disk usage tool
        static void AnalyzeCommandLineArgs(Configuration.CLI_Configuration analyzer, string[] args)
        {
            try
            {
                if (args.Length == 2)
                {

                    if (analyzer.ModeSelected == false && (args[0].Equals("-s")) || (args[0].Equals("-d")) || (args[0].Equals("-b")))
                    {
                        analyzer.ModeStr = args[0];
                        analyzer.ModeSelected = true;
                    }
                    else
                    {
                        throw new InvalidProgramException("");
                    }

                    if (analyzer.ModeSelected == true && analyzer.PathSelected == false)
                    {
                        if (IsString(args[1]))
                        {
                            bool validateDirectory = ValidateDirectory(args[1]);
                            if (validateDirectory == false)
                            {
                                throw new InvalidProgramException("");
                            }
                            else
                            {
                                analyzer.PathStr = args[1];
                                analyzer.PathSelected = true;
                            }
                        }
                        else
                        {
                            throw new InvalidProgramException("");
                        }

                    }

                    try
                    {
                        // Just to double check
                        if (ValidateDirectory(args[1]) == false)
                        {
                            throw new InvalidProgramException("");
                        }
                        else
                        {
                            analyzer.PathStr = args[1];
                            analyzer.PathSelected = true;
                        }


                        if (string.IsNullOrEmpty(analyzer.ModeStr) || string.IsNullOrEmpty(analyzer.PathStr))
                        {
                            throw new InvalidProgramException("");
                        }

                    }
                    catch (Exception)
                    {
                        PrintUsageMessage();
                        System.Environment.Exit(-1);
                    }
                }
                else
                {
                    throw new InvalidProgramException("");
                }
            }
            catch (InvalidProgramException)
            {
                PrintUsageMessage();
                System.Environment.Exit(-1);
            }
        }

        // Run the disk usage tool based on the selected mode
        static void RunDiskUsageTool(Configuration.CLI_Configuration analyzer)
        {

            Runner.IMode_Runner sequentialRunner = new Runner.SequentialMode();
            Runner.IMode_Runner parallelRunner = new Runner.ParallelMode();

            Console.WriteLine($"Directory '{analyzer.PathStr}':\n");

            if (analyzer.ModeStr.Equals("-s"))
            {
                // Measure and display the time for the sequential version
                Stopwatch sequentialWatch = Stopwatch.StartNew();
                var sequentialResult = sequentialRunner.RunMode(analyzer.PathStr);
                sequentialWatch.Stop();

                // Display the results of the sequential version
                sequentialRunner.DisplayResults(sequentialWatch.Elapsed.TotalSeconds, sequentialResult);
            }
            else if (analyzer.ModeStr.Equals("-d"))
            {
                Stopwatch parallelWatch = Stopwatch.StartNew();
                var parallelResult = parallelRunner.RunMode(analyzer.PathStr);
                parallelWatch.Stop();

                parallelRunner.DisplayResults(parallelWatch.Elapsed.TotalSeconds, parallelResult);
            }
            else if (analyzer.ModeStr.Equals("-b"))
            {
                // Measure and display the time for the parallel version
                Stopwatch parallelWatch = Stopwatch.StartNew();
                var parallelResult = parallelRunner.RunMode(analyzer.PathStr);
                parallelWatch.Stop();

                // Measure and display the time for the sequential version
                Stopwatch sequentialWatch = Stopwatch.StartNew();
                var sequentialResult = sequentialRunner.RunMode(analyzer.PathStr);
                sequentialWatch.Stop();

                // Display the results of the parallel version
                parallelRunner.DisplayResults(parallelWatch.Elapsed.TotalSeconds, parallelResult);

                // Display the results of the sequential version
                sequentialRunner.DisplayResults(sequentialWatch.Elapsed.TotalSeconds, sequentialResult);
            }
            Console.WriteLine("\n");
        }

        // Main entry point of the application{
        static void Main(string[] args)
        {
            Configuration.CLI_Configuration analyzer = new Configuration.CLI_Configuration();

            // Analyze command-line arguments and run the disk usage tool
            AnalyzeCommandLineArgs(analyzer, args);
            RunDiskUsageTool(analyzer);
        }
    }
}