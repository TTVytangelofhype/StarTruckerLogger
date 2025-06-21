// ------------------------------------------------------------
// StarTruckerLogger © 2025 by TTVytangelofhype
// You are free to modify the code, but not to remove credit,
// redistribute under your name, or sell it as your own.
// ------------------------------------------------------------
using System;
using System.IO;

namespace StarTruckerLogger
{
    public static class JobWatcher
    {
        private static FileSystemWatcher watcher;

        public static void StartWatching()
        {
            watcher = new FileSystemWatcher
            {
                Path = "C:\\Games\\StarTrucker\\logs",
                Filter = "latest_job.txt",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            watcher.Changed += OnNewJobLogged;
        }

        private static void OnNewJobLogged(object sender, FileSystemEventArgs e)
        {
            try
            {
                Console.WriteLine($"Watcher triggered: {e.FullPath}"); // Debug Line 1

                var content = File.ReadAllText(e.FullPath);
                Console.WriteLine($"New content: {content}"); // Debug Line 2

                if (!string.IsNullOrWhiteSpace(content))
                {
                    JobLogger.LogToFile($"[MOD] {DateTime.Now} | {content.Trim()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in watcher: " + ex.Message);
            }
        }
    }
}
