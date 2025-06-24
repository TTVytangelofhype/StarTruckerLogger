// File: MainForm.cs
// Description: The main job logging window.

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarTruckerLogger
{
    public class MainForm : Form
    {
        private TextBox txtCargo;
        private TextBox txtOrigin;
        private TextBox txtDestination;
        private TextBox txtDistance;
        private TextBox txtPayment;
        private Button btnLogJob;
        private Button btnViewLog;

        public MainForm()
        {
            // Update the title to show who is logged in
            this.Text = $"Star Trucker Logger - [{ApiClient.CurrentUser.Username}]";
            this.Width = 400;
            this.Height = 300;

            InitializeControls();
            JobWatcher.StartWatching();
        }

        private void InitializeControls()
        {
            txtCargo = new TextBox { Left = 120, Top = 20, Width = 200 };
            txtOrigin = new TextBox { Left = 120, Top = 50, Width = 200 };
            txtDestination = new TextBox { Left = 120, Top = 80, Width = 200 };
            txtDistance = new TextBox { Left = 120, Top = 110, Width = 200 };
            txtPayment = new TextBox { Left = 120, Top = 140, Width = 200 };

            btnLogJob = new Button { Text = "Log Job to VTC", Left = 50, Top = 180, Width = 120 };
            btnViewLog = new Button { Text = "View Local Logs", Left = 200, Top = 180, Width = 120 };

            btnLogJob.Click += async (s, e) => await btnLogJob_Click();
            btnViewLog.Click += btnViewLog_Click;

            this.Controls.Add(new Label { Text = "Cargo:", Left = 20, Top = 20 });
            this.Controls.Add(txtCargo);
            this.Controls.Add(new Label { Text = "Origin:", Left = 20, Top = 50 });
            this.Controls.Add(txtOrigin);
            this.Controls.Add(new Label { Text = "Destination:", Left = 20, Top = 80 });
            this.Controls.Add(txtDestination);
            this.Controls.Add(new Label { Text = "Distance (LY):", Left = 20, Top = 110 });
            this.Controls.Add(txtDistance);
            this.Controls.Add(new Label { Text = "Payment (Cr):", Left = 20, Top = 140 });
            this.Controls.Add(txtPayment);

            this.Controls.Add(btnLogJob);
            this.Controls.Add(btnViewLog);
        }

        private async Task btnLogJob_Click()
        {
            btnLogJob.Text = "Syncing...";
            btnLogJob.Enabled = false;

            // Log to the website via the API
            bool success = await ApiClient.LogJobAsync(
                txtOrigin.Text,
                txtDestination.Text,
                txtCargo.Text,
                txtDistance.Text,
                txtPayment.Text
            );

            if (success)
            {
                MessageBox.Show("Job successfully synced with the VTC portal!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Also log locally as a backup
                string log = $"[SYNCED] [{DateTime.Now}] | Cargo: {txtCargo.Text} | From: {txtOrigin.Text} -> {txtDestination.Text}";
                JobLogger.LogToFile(log);
                // Clear fields for next job
                txtCargo.Clear();
                txtOrigin.Clear();
                txtDestination.Clear();
                txtDistance.Clear();
                txtPayment.Clear();
            }
            else
            {
                MessageBox.Show("Failed to sync job with the VTC portal. Please check your connection and try again.", "Sync Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnLogJob.Text = "Log Job to VTC";
            btnLogJob.Enabled = true;
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            var logs = JobLogger.ReadAll();
            MessageBox.Show(string.Join(Environment.NewLine, logs), "Local Job Logs");
        }
    }
}