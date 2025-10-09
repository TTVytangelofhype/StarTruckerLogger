using System.Drawing;
using System.Windows.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        InitializeCustomComponents();
    }

    private void InitializeCustomComponents()
    {
        // Set the form properties for a borderless window
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = Color.Black;
        this.Size = new Size(800, 600); // Adjust as needed
        this.StartPosition = FormStartPosition.CenterScreen;

        // Create a custom panel for the main window frame
        Panel mainFramePanel = new Panel();
        mainFramePanel.Dock = DockStyle.Fill;
        mainFramePanel.BackgroundImage = Properties.Resources.MetalFrameTexture; // Replace with your frame image
        mainFramePanel.BackgroundImageLayout = ImageLayout.Stretch;
        this.Controls.Add(mainFramePanel);

        // Create a panel for the content (the space view and UI)
        Panel contentPanel = new Panel();
        contentPanel.Size = new Size(760, 560); // Adjust to fit inside the frame
        contentPanel.Location = new Point(20, 20); // Position inside the frame
        contentPanel.BackColor = Color.Black;
        contentPanel.BackgroundImage = Properties.Resources.SpaceBackground; // Replace with your space background
        contentPanel.BackgroundImageLayout = ImageLayout.Zoom;
        mainFramePanel.Controls.Add(contentPanel);

        // Add a panel for the "Cargo Manifest" display
        Panel manifestPanel = new Panel();
        manifestPanel.Size = new Size(300, 100); // Adjust size
        manifestPanel.Location = new Point(30, 30); // Position within the content panel
        manifestPanel.BackColor = Color.FromArgb(20, 20, 20); // Dark background
        manifestPanel.BorderStyle = BorderStyle.FixedSingle;
        contentPanel.Controls.Add(manifestPanel);

        // Add a Label for "CARGO MANIFEST v.2.7"
        Label manifestTitleLabel = new Label();
        manifestTitleLabel.Text = "CARGO MANIFEST v.2.7";
        manifestTitleLabel.ForeColor = Color.LimeGreen;
        manifestTitleLabel.Font = new Font("Consolas", 12, FontStyle.Regular);
        manifestTitleLabel.AutoSize = true;
        manifestTitleLabel.Location = new Point(10, 10);
        manifestPanel.Controls.Add(manifestTitleLabel);

        // Add a Label for the route
        Label routeLabel = new Label();
        routeLabel.Text = "ROUTE: EPSILON-7 -> ANDROMDA PRIME";
        routeLabel.ForeColor = Color.LimeGreen;
        routeLabel.Font = new Font("Consolas", 10, FontStyle.Regular);
        routeLabel.AutoSize = true;
        routeLabel.Location = new Point(10, 40);
        manifestPanel.Controls.Add(routeLabel);

        // Implement the minimize, maximize, and close buttons
        // You would need to create custom button images or draw them
        // Here, we'll just use simple buttons for demonstration
        Button closeButton = new Button();
        closeButton.Text = "X";
        closeButton.Location = new Point(this.Width - 40, 5);
        closeButton.Click += (sender, e) => { this.Close(); };
        mainFramePanel.Controls.Add(closeButton);

        // Add more UI elements as needed based on the design
        // For example, you would add PictureBox controls to hold images
        // and other labels or text boxes for different sections.
    }
}