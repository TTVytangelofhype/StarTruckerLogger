// File: LoginForm.cs
// Description: The login window for the application. (Updated with fix for getting stuck)

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks; // Ensure this is included

namespace StarTruckerLogger
{
    public class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private LinkLabel lnkForgotPassword;
        private Label lblStatus;

        public LoginForm()
        {
            this.Text = "Star Trucker VTC Login";
            this.Width = 350;
            this.Height = 250;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;

            InitializeControls();
        }

        private void InitializeControls()
        {
            // Username
            this.Controls.Add(new Label { Text = "Username:", Left = 30, Top = 30 });
            txtUsername = new TextBox { Left = 120, Top = 30, Width = 180 };
            this.Controls.Add(txtUsername);

            // Password
            this.Controls.Add(new Label { Text = "Password:", Left = 30, Top = 70 });
            txtPassword = new TextBox { Left = 120, Top = 70, Width = 180, UseSystemPasswordChar = true };
            this.Controls.Add(txtPassword);

            // Login Button
            btnLogin = new Button { Text = "Login", Left = 120, Top = 110, Width = 100 };
            btnLogin.Click += async (s, e) => await btnLogin_Click();
            this.Controls.Add(btnLogin);
            this.AcceptButton = btnLogin; // Allow pressing Enter to log in

            // Forgot Password Link
            lnkForgotPassword = new LinkLabel { Text = "Forgot Password?", Left = 120, Top = 150, Width = 150 };
            lnkForgotPassword.Click += lnkForgotPassword_Click;
            this.Controls.Add(lnkForgotPassword);

            // Status Label
            lblStatus = new Label { Text = "", Left = 30, Top = 180, Width = 270, ForeColor = Color.Red };
            this.Controls.Add(lblStatus);
        }

        private async Task btnLogin_Click()
        {
            lblStatus.Text = "Logging in...";
            btnLogin.Enabled = false;

            try
            {
                var result = await ApiClient.LoginAsync(txtUsername.Text, txtPassword.Text);

                if (result.Success)
                {
                    this.DialogResult = DialogResult.OK; // Signal success
                    this.Close();
                }
                else
                {
                    lblStatus.Text = $"Login Failed: {result.Message}";
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors during the login process
                lblStatus.Text = $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                // This 'finally' block will ALWAYS run, even if the login fails or times out.
                // This ensures the button is always re-enabled, preventing the form from getting stuck.
                if (this.DialogResult != DialogResult.OK)
                {
                    btnLogin.Enabled = true;
                }
            }
        }

        private void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the forgot password link on your website
                Process.Start("https://startruckervtc.co.uk/forgot_password.php");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open link. Please visit the website manually.\nError: {ex.Message}", "Error");
            }
        }
    }
}
