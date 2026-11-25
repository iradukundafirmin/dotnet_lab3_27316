using System;
using System.Drawing;
using System.Windows.Forms;
using CoworkingApp.Controllers;
using CoworkingApp.Models;

namespace CoworkingApp.Views
{
    public class DesksForm : Form
    {
        private DeskController controller = new DeskController();

        private DataGridView grid = new DataGridView();
        private TextBox txtName = new TextBox();
        private ComboBox cmbType = new ComboBox();
        private NumericUpDown numPrice = new NumericUpDown();
        private int selectedId = 0;   // 0 = nothing selected

        public DesksForm()
        {
            Text = "Desks / Rooms";
            ClientSize = new Size(580, 400);
            StartPosition = FormStartPosition.CenterParent;

            // Grid
            grid.SetBounds(10, 10, 560, 220);
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.CellClick += Grid_CellClick;
            Controls.Add(grid);

            // Type list
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Items.AddRange(new object[] { "Hot desk", "Private office", "Meeting room" });
            cmbType.SelectedIndex = 0;

            // Price box
            numPrice.DecimalPlaces = 2;
            numPrice.Maximum = 100000;

            // Fields
            AddField("Name", txtName, 245);
            AddField("Type", cmbType, 275);
            AddField("Price / day", numPrice, 305);

            // Buttons
            AddButton("Add", 10, BtnAdd_Click);
            AddButton("Update", 100, BtnUpdate_Click);
            AddButton("Delete", 190, BtnDelete_Click);
            AddButton("Clear", 280, (s, e) => ClearFields());

            LoadData();
        }

        // ----- helpers to build the form -----
        private void AddField(string label, Control box, int y)
        {
            var lbl = new Label();
            lbl.Text = label;
            lbl.SetBounds(10, y + 3, 80, 20);
            Controls.Add(lbl);

            box.SetBounds(100, y, 250, 25);
            Controls.Add(box);
        }

        private void AddButton(string text, int x, EventHandler onClick)
        {
            var btn = new Button();
            btn.Text = text;
            btn.SetBounds(x, 350, 80, 30);
            btn.Click += onClick;
            Controls.Add(btn);
        }

        // ----- view logic -----
        private void LoadData()
        {
            grid.DataSource = controller.GetAll();
            ClearFields();
        }

        private void ClearFields()
        {
            selectedId = 0;
            txtName.Text = "";
            cmbType.SelectedIndex = 0;
            numPrice.Value = 0;
            grid.ClearSelection();
        }

        private Desk ReadForm()
        {
            return new Desk
            {
                Id = selectedId,
                Name = txtName.Text,
                Type = cmbType.Text,
                PricePerDay = numPrice.Value
            };
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var d = (Desk)grid.Rows[e.RowIndex].DataBoundItem;
            selectedId = d.Id;
            txtName.Text = d.Name;
            cmbType.Text = d.Type;
            numPrice.Value = d.PricePerDay;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string error = controller.Add(ReadForm());
            if (error != null) { MessageBox.Show(error); return; }
            LoadData();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0) { MessageBox.Show("Select a desk in the list first."); return; }
            string error = controller.Update(ReadForm());
            if (error != null) { MessageBox.Show(error); return; }
            LoadData();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0) { MessageBox.Show("Select a desk in the list first."); return; }
            var answer = MessageBox.Show("Delete this desk and all its bookings?",
                "Confirm", MessageBoxButtons.YesNo);
            if (answer != DialogResult.Yes) return;
            controller.Delete(selectedId);
            LoadData();
        }
    }
}
