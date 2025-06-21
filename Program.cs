// ------------------------------------------------------------
// StarTruckerLogger © 2025 by TTVytangelofhype
// You are free to modify the code, but not to remove credit,
// redistribute under your name, or sell it as your own.
// ------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace StarTruckerLogger
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles(); // Optional but recommended
            Application.SetCompatibleTextRenderingDefault(false); // Also optional
            Application.Run(new MainForm()); // Start the GUI
        }
    }
}
