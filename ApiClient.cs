// File: ApiClient.cs
// Description: Handles all communication with the VTC website API.

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StarTruckerLogger
{
    public static class ApiClient
    {
        // This URL points to your live website's API endpoint.
        private static readonly string ApiBaseUrl = "https://startruckervtc.co.uk/api.php";
        private static readonly HttpClient client = new HttpClient();

        // Stores the logged-in user's session data
        public static UserSession CurrentUser { get; private set; }

        public static async Task<LoginResult> LoginAsync(string username, string password)
        {
            var credentials = new { action = "login", username, password };
            var jsonContent = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(ApiBaseUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (result.status == "success")
                {
                    // Store user data upon successful login
                    CurrentUser = new UserSession
                    {
                        UserId = result.data.user_id,
                        Username = result.data.username,
                        VtcId = result.data.vtc_id
                    };
                    return new LoginResult { Success = true };
                }
                else
                {
                    return new LoginResult { Success = false, Message = result.message };
                }
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Message = $"API Connection Error: {ex.Message}" };
            }
        }

        public static async Task<bool> LogJobAsync(string origin, string destination, string cargo, string distance, string pay)
        {
            if (CurrentUser == null) return false; // Not logged in

            var jobData = new
            {
                action = "log_job",
                user_id = CurrentUser.UserId,
                vtc_id = CurrentUser.VtcId,
                origin,
                destination,
                cargo,
                distance = double.Parse(distance),
                pay = double.Parse(pay)
            };
            var jsonContent = JsonConvert.SerializeObject(jobData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(ApiBaseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UserSession
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int VtcId { get; set; }
    }
}