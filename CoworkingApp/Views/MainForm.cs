using System;
using System.Drawing;
using System.Windows.Forms;

namespace CoworkingApp.Views
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Co-working Space Manager";
            ClientSize = new Size(300, 230);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var title = new Label();
            title.Text = "Co-working Space Manager";
            title.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.SetBounds(10, 15, 280, 30);
            Controls.Add(title);

            AddButton("Members", 60, (s, e) => new MembersForm().ShowDialog());
            AddButton("Desks / Rooms", 110, (s, e) => new DesksForm().ShowDialog());
            AddButton("Bookings", 160, (s, e) => new BookingsForm().ShowDialog());
        }

        private void AddButton(string text, int y, EventHandler onClick)
        {
            var btn = new Button();
            btn.Text = text;
            btn.SetBounds(60, y, 180, 35);
            btn.Click += onClick;
            Controls.Add(btn);
        }
    }
}
