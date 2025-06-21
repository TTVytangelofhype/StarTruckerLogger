// ------------------------------------------------------------
// StarTruckerLogger © 2025 by TTVytangelofhype
// You are free to modify the code, but not to remove credit,
// redistribute under your name, or sell it as your own.
// ------------------------------------------------------------
using System;
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
            this.Text = "Star Trucker Logger";
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

            btnLogJob = new Button { Text = "Log Job", Left = 50, Top = 180, Width = 120 };
            btnViewLog = new Button { Text = "View Logs", Left = 200, Top = 180, Width = 120 };

            btnLogJob.Click += btnLogJob_Click;
            btnViewLog.Click += btnViewLog_Click;

            this.Controls.Add(new Label { Text = "Cargo:", Left = 20, Top = 20 });
            this.Controls.Add(txtCargo);
            this.Controls.Add(new Label { Text = "Origin:", Left = 20, Top = 50 });
            this.Controls.Add(txtOrigin);
            this.Controls.Add(new Label { Text = "Destination:", Left = 20, Top = 80 });
            this.Controls.Add(txtDestination);
            this.Controls.Add(new Label { Text = "Distance:", Left = 20, Top = 110 });
            this.Controls.Add(txtDistance);
            this.Controls.Add(new Label { Text = "Payment:", Left = 20, Top = 140 });
            this.Controls.Add(txtPayment);

            this.Controls.Add(btnLogJob);
            this.Controls.Add(btnViewLog);
        }

        private void btnLogJob_Click(object sender, EventArgs e)
        {
            string cargo = txtCargo.Text;
            string origin = txtOrigin.Text;
            string destination = txtDestination.Text;
            string distance = txtDistance.Text;
            string payment = txtPayment.Text;

            string log = $"[{DateTime.Now}] | Cargo: {cargo} | From: {origin} -> {destination} | Distance: {distance} LY | Pay: {payment} cr";
            JobLogger.LogToFile(log);
            MessageBox.Show("Job logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            var logs = JobLogger.ReadAll();
            MessageBox.Show(string.Join(Environment.NewLine, logs), "Job Logs");
        }
    }
}
