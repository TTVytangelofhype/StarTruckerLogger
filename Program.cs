// File: Program.cs
// Description: Main entry point for the application.

using System;
using System.Windows.Forms;

namespace StarTruckerLogger
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the login form as a dialog
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // If login was successful, open the main application window
                Application.Run(new MainForm());
            }
            // If login fails or is cancelled, the application will exit.
        }
    }
}
