using System;
using System.Drawing;
using System.Windows.Forms;
using CoworkingApp.Controllers;
using CoworkingApp.Models;

namespace CoworkingApp.Views
{
    public class BookingsForm : Form
    {
        private BookingController controller = new BookingController();
        private MemberController memberController = new MemberController();
        private DeskController deskController = new DeskController();

        private DataGridView grid = new DataGridView();
        private ComboBox cmbMember = new ComboBox();
        private ComboBox cmbDesk = new ComboBox();
        private DateTimePicker dtpDate = new DateTimePicker();
        private int selectedId = 0;   // 0 = nothing selected

        public BookingsForm()
        {
            Text = "Bookings";
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

            // Combo boxes show the name but remember the Id
            cmbMember.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMember.DisplayMember = "Name";
            cmbMember.ValueMember = "Id";

            cmbDesk.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDesk.DisplayMember = "Name";
            cmbDesk.ValueMember = "Id";

            dtpDate.Format = DateTimePickerFormat.Short;

            // Fields
            AddField("Member", cmbMember, 245);
            AddField("Desk / Room", cmbDesk, 275);
            AddField("Date", dtpDate, 305);

            // Buttons
            AddButton("Add", 10, BtnAdd_Click);
            AddButton("Update", 100, BtnUpdate_Click);
            AddButton("Delete", 190, BtnDelete_Click);
            AddButton("Clear", 280, (s, e) => ClearFields());

            LoadCombos();
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
        private void LoadCombos()
        {
            cmbMember.DataSource = memberController.GetAll();
            cmbDesk.DataSource = deskController.GetAll();
        }

        private void LoadData()
        {
            grid.DataSource = controller.GetAll();

            // Hide the raw Id columns and format the date
            grid.Columns["MemberId"].Visible = false;
            grid.Columns["DeskId"].Visible = false;
            grid.Columns["BookingDate"].DefaultCellStyle.Format = "yyyy-MM-dd";

            ClearFields();
        }

        private void ClearFields()
        {
            selectedId = 0;
            dtpDate.Value = DateTime.Today;
            grid.ClearSelection();
        }

        // Returns null (and shows a message) if a member or desk is not chosen
        private Booking ReadForm()
        {
            if (cmbMember.SelectedValue == null || cmbDesk.SelectedValue == null)
            {
                MessageBox.Show("Please add at least one member and one desk first.");
                return null;
            }

            return new Booking
            {
                Id = selectedId,
                MemberId = Convert.ToInt32(cmbMember.SelectedValue),
                DeskId = Convert.ToInt32(cmbDesk.SelectedValue),
                BookingDate = dtpDate.Value.Date
            };
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var b = (Booking)grid.Rows[e.RowIndex].DataBoundItem;
            selectedId = b.Id;
            cmbMember.SelectedValue = b.MemberId;
            cmbDesk.SelectedValue = b.DeskId;
            dtpDate.Value = b.BookingDate;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var b = ReadForm();
            if (b == null) return;
            string error = controller.Add(b);
            if (error != null) { MessageBox.Show(error); return; }
            LoadData();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0) { MessageBox.Show("Select a booking in the list first."); return; }
            var b = ReadForm();
            if (b == null) return;
            string error = controller.Update(b);
            if (error != null) { MessageBox.Show(error); return; }
            LoadData();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0) { MessageBox.Show("Select a booking in the list first."); return; }
            var answer = MessageBox.Show("Delete this booking?", "Confirm", MessageBoxButtons.YesNo);
            if (answer != DialogResult.Yes) return;
            controller.Delete(selectedId);
            LoadData();
        }
    }
}
