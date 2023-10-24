using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Runner
{
    interface IMode_Runner
    {
        (long folders, long files, long size) RunMode(string directoryPath);
        void DisplayResults(double elapsedTime, (long folders, long files, long size) result);
    }

    // Class for running disk usage calculation in parallel mode
    class ParallelMode : IMode_Runner
    {
        private readonly object lockObj = new object();

        // Method to run the mode in parallel on a directory
        public (long folders, long files, long size) RunMode(string directoryPath)
        {
            (long folders, long files, long size) result = (0, 0, 0);

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

                if (!directoryInfo.Exists)
                {
                    return result; // Return zeros if the directory doesn't exist
                }

                Parallel.ForEach(directoryInfo.GetDirectories(), subdirectoryInfo =>
                {
                    try
                    {
                        var subResult = RunMode(subdirectoryInfo.FullName);

                        lock (lockObj)
                        {
                            result.folders += 1 + subResult.folders;
                            result.files += subResult.files;
                            result.size += subResult.size;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });

                Parallel.ForEach(directoryInfo.GetFiles(), fileInfo =>
                {
                    try
                    {
                        lock (lockObj)
                        {
                            result.files++;
                            result.size += fileInfo.Length;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
            catch (Exception)
            {
            }

            return result;
        }

        public void DisplayResults(double elapsedTime, (long folders, long files, long size) result)
        {
            Console.WriteLine($"Parallel Calculated in: {elapsedTime}s");
            Console.WriteLine($"{result.folders:N0} folders, {result.files:N0} files, {result.size:N0} bytes\n");
        }
    }

    // Class for running disk usage calculation in sequential mode
    class SequentialMode : IMode_Runner
    {
        private readonly object lockObj = new object();

        // Method to run the mode sequentially on a directory
        public (long folders, long files, long size) RunMode(string directoryPath)
        {
            (long folders, long files, long size) result = (0, 0, 0);

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

                if (!directoryInfo.Exists)
                {
                    return result; // Return zeros if the directory doesn't exist
                }

                foreach (var subdirectoryInfo in directoryInfo.GetDirectories())
                {
                    try
                    {
                        var subResult = RunMode(subdirectoryInfo.FullName);

                        lock (lockObj)
                        {
                            result.folders += 1 + subResult.folders;
                            result.files += subResult.files;
                            result.size += subResult.size;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                foreach (var fileInfo in directoryInfo.GetFiles())
                {
                    try
                    {
                        lock (lockObj)
                        {
                            result.files++;
                            result.size += fileInfo.Length;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public void DisplayResults(double elapsedTime, (long folders, long files, long size) result)
        {
            Console.WriteLine($"Sequential Calculated in: {elapsedTime}s");
            Console.WriteLine($"{result.folders:N0} folders, {result.files:N0} files, {result.size:N0} bytes\n");
        }

    }
}
