// Star Trucker VTC Client
// ========================
// This is a C# Console Application that connects to your VTC website's API.
//
// HOW TO COMPILE AND USE:
// 1. Open Visual Studio and create a new project: "Console App".
// 2. Name your project "StarTruckerClient".
// 3. Replace the contents of the generated "Program.cs" file with this entire code.
// 4. Install the required package:
//    - Go to "Tools" > "NuGet Package Manager" > "Package Manager Console".
//    - In the console that appears, type: Install-Package Newtonsoft.Json
//    - Press Enter.
// 5. Build the program:
//    - Go to "Build" > "Build Solution" (or press Ctrl+Shift+B).
// 6. Find the executable:
//    - Right-click your project in the "Solution Explorer" and choose "Open Folder in File Explorer".
//    - Navigate into the "bin/Debug/netX.X" folder (the .net version may vary).
//    - The "StarTruckerClient.exe" file is your ready-to-use program. Distribute this to your drivers.

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

public class StarTruckerClient
{
    // This URL is now pointing to your live website's API endpoint.
    private static readonly string ApiBaseUrl = "https://startruckervtc.co.uk/api.php";

    private static readonly HttpClient client = new HttpClient();

    // Player session information
    private static string username;
    private static int userId;
    private static int vtcId;

    static async Task Main(string[] args)
    {
        Console.Title = "Star Trucker VTC Client v1.1";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
        ***********************************
        * STAR TRUCKER VTC CLIENT        *
        ***********************************
        ");
        Console.ResetColor();

        // 1. Handle User Login
        if (!await HandleLogin())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nLogin failed after multiple attempts.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            return;
        }

        // 2. Main Game Loop
        await MainMenuLoop();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nThank you for driving with us, Commander! Client shutting down.");
        Console.ResetColor();
    }

    private static async Task<bool> HandleLogin()
    {
        for (int i = 0; i < 3; i++) // Allow 3 login attempts
        {
            Console.WriteLine("\nPlease log in with your website credentials.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Hint: Type 'forgot' as the username to open the password reset link.");
            Console.ResetColor();
            Console.Write("Enter Username: ");
            string u = Console.ReadLine();

            if (u.ToLower() == "forgot")
            {
                OpenUrl("https://startruckervtc.co.uk/forgot_password.php");
                Console.WriteLine("Password reset link opened in your browser. Please restart the client after resetting your password.\n");
                continue; // Go back to the start of the loop
            }

            Console.Write("Enter Password: ");
            string p = Console.ReadLine();

            var credentials = new { action = "login", username = u, password = p };
            var jsonContent = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(ApiBaseUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (result.status == "success")
                {
                    username = result.data.username;
                    userId = result.data.user_id;
                    vtcId = result.data.vtc_id;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nLogin successful! Welcome, {username}.\n");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Login failed: {result.message}\n");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAPI Connection Error: Could not connect to the server at {ApiBaseUrl}.");
                Console.WriteLine($"Details: {ex.Message}\n");
                Console.ResetColor();
            }
        }
        return false;
    }

    private static async Task MainMenuLoop()
    {
        while (true)
        {
            Console.WriteLine("--- VTC Command Console ---");
            Console.WriteLine("1. Start a New Haul");
            Console.WriteLine("2. Exit Client");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                await StartHaul();
            }
            else if (choice == "2")
            {
                break;
            }
        }
    }

    private static async Task StartHaul()
    {
        // Get job details from the user
        Console.WriteLine("\n--- NEW HAUL CONTRACT ---");
        Console.Write("Enter Origin System: ");
        string origin = Console.ReadLine();
        Console.Write("Enter Destination System: ");
        string destination = Console.ReadLine();
        Console.Write("Enter Cargo Manifest: ");
        string cargo = Console.ReadLine();
        Console.Write("Enter Distance (LY): ");
        double distance = double.Parse(Console.ReadLine());
        Console.Write("Enter Payout (Cr): ");
        double pay = double.Parse(Console.ReadLine());

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nContract accepted. Hauling {cargo} from {origin} to {destination}. Good luck, driver.");
        Console.ResetColor();

        double distanceTraveled = 0;
        Random rand = new Random();

        // Simulate the drive, updating the server every 4 seconds
        while (distanceTraveled < distance)
        {
            distanceTraveled += distance * (rand.NextDouble() * 0.05 + 0.05);
            if (distanceTraveled > distance) distanceTraveled = distance;

            string currentLocation = $"Hyperspace Jump | En route to {destination}";
            string statusMessage = $"Progress: {distanceTraveled:F1} / {distance:F1} LY";

            Console.WriteLine(statusMessage);
            await UpdateStatusOnServer(currentLocation, statusMessage);

            Thread.Sleep(4000);
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nJob complete! Arrived at {destination}. Finalizing paperwork...");
        Console.ResetColor();
        await UpdateStatusOnServer(destination, "Docked. Job Complete.");

        await LogJobToDatabase(origin, destination, cargo, distance, pay);
    }

    private static async Task UpdateStatusOnServer(string location, string status)
    {
        var statusUpdate = new
        {
            action = "update_status",
            user_id = userId,
            vtc_id = vtcId,
            username = username,
            location = location,
            status = status
        };
        var jsonContent = JsonConvert.SerializeObject(statusUpdate);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        try { await client.PostAsync(ApiBaseUrl, content); } catch { /* Fail silently */ }
    }

    private static async Task LogJobToDatabase(string origin, string destination, string cargo, double distance, double pay)
    {
        var jobData = new
        {
            action = "log_job",
            user_id = userId,
            vtc_id = vtcId,
            origin = origin,
            destination = destination,
            cargo = cargo,
            distance = distance,
            pay = pay
        };
        var jsonContent = JsonConvert.SerializeObject(jobData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync(ApiBaseUrl, content);
            if (response.IsSuccessStatusCode) Console.WriteLine("Successfully synced job log with VTC portal!");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Could not sync job log: {ex.Message}");
            Console.ResetColor();
        }
    }

    // Helper function to open a URL in the default web browser
    private static void OpenUrl(string url)
    {
        try { Process.Start(url); }
        catch
        {
            // Handle cases where Process.Start is not available (e.g., non-Windows OS)
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else { throw; }
        }
    }
}
