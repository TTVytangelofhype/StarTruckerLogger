using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StarTruckerLogger
{
    class DeliveryLogger
    {
        private static FileSystemWatcher watcher;
        private static string lastStatus = "";
        private static DateTime? deliveryStartTime = null;
        private static string currentCargo = "";
        private static string currentOrigin = "";
        private static string currentDestination = "";

        static async Task Main(string[] args)
        {
            Console.WriteLine("🚀 Star Trucker Delivery Logger with Duration Tracker");
            StartWatching();
            Console.WriteLine("🛰 Watching for delivery events... Press Enter to exit.");
            Console.ReadLine();
        }

        private static void StartWatching()
        {
            string path = @"C:\Games\StarTrucker\logs";
            string file = "latest_status.txt";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            watcher = new FileSystemWatcher
            {
                Path = path,
                Filter = file,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            watcher.Changed += async (s, e) => await OnStatusChanged(e);
        }

        private static async Task OnStatusChanged(FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(100); // Wait for file write
                string content = File.ReadAllText(e.FullPath).Trim();

                if (content == lastStatus)
                    return;

                lastStatus = content;

                if (content.Contains("Delivery Started:"))
                {
                    deliveryStartTime = DateTime.Now;

                    string[] parts = content.Split("Cargo:");
                    if (parts.Length > 1)
                    {
                        var jobDetails = parts[1].Trim().Split('|');
                        currentCargo = jobDetails[0].Trim();
                        var route = jobDetails[1].Replace("From:", "").Trim().Split("->");
                        currentOrigin = route[0].Trim();
                        currentDestination = route[1].Trim();
                    }

                    Log($"🚀 Delivery Started: {currentCargo} ({currentOrigin} ➜ {currentDestination})");
                }
                else if (content.Contains("Delivery Completed:") && deliveryStartTime != null)
                {
                    DateTime endTime = DateTime.Now;
                    TimeSpan duration = endTime - deliveryStartTime.Value;

                    string[] parts = content.Split('|');
                    string distanceStr = parts[0].Replace("Distance:", "").Replace("LY", "").Trim();
                    string paymentStr = parts[1].Replace("Pay:", "").Replace("cr", "").Trim();

                    double.TryParse(distanceStr, out double distance);
                    double.TryParse(paymentStr, out double payment);

                    var job = new
                    {
                        cargo = currentCargo,
                        origin = currentOrigin,
                        destination = currentDestination,
                        distance = distance,
                        payment = payment,
                        duration_minutes = Math.Round(duration.TotalMinutes, 2)
                    };

                    string json = JsonSerializer.Serialize(job);
                    var http = new HttpClient();
                    var response = await http.PostAsync(
                        "http://localhost/api_submit_log.php",
                        new StringContent(json, Encoding.UTF8, "application/json")
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        Log($"✅ Delivery Completed: {currentCargo} in {duration.TotalMinutes:F1} mins | {distance} LY | {payment} cr (Posted)");
                    }
                    else
                    {
                        Log("❌ Failed to post job to API.");
                    }

                    // Reset
                    deliveryStartTime = null;
                    currentCargo = currentOrigin = currentDestination = "";
                }
            }
            catch (Exception ex)
            {
                Log("⚠️ Watcher error: " + ex.Message);
            }
        }

        private static void Log(string message)
        {
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            Console.WriteLine(entry);
            File.AppendAllText("job_log.txt", entry + Environment.NewLine);
        }
    }
}
