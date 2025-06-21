// ------------------------------------------------------------
// StarTruckerLogger © 2025 by TTVytangelofhype
// You are free to modify the code, but not to remove credit,
// redistribute under your name, or sell it as your own.
// ------------------------------------------------------------
using System;
using System.IO;

namespace StarTruckerLogger
{
    public static class JobLogger
    {
        private static string logFilePath = "latest_job.txt";

        public static void LogToFile(string logEntry)
        {
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }

        public static string[] ReadAll()
        {
            return File.Exists(logFilePath) ? File.ReadAllLines(logFilePath) : new string[] { "No log found." };
        }
    }
}
