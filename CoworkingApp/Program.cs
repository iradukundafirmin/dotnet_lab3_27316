using System;
using System.Windows.Forms;
using CoworkingApp.Models;
using CoworkingApp.Views;

namespace CoworkingApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Creates the database file and tables on first run
            Database.Initialize();

            Application.Run(new MainForm());
        }
    }
}
