using System.Collections.Generic;
using System.Windows.Forms;
using System;
using Controller;
using Main.Model;
using Menu = Main.Model.Menu;

namespace View
{
    public partial class MenuManager : Form
    {
        private MenuController _controller;

        public MenuManager()
        {
            InitializeComponent();
            _controller = new MenuController(this);
            _controller.PopulateDataGridView();
            this.dataGridView1.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_RowHeaderMouseClick);
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _controller.RowHeaderMouseClick(e.RowIndex);
        }

        public void PopulateDataGridView(List<Menu> daftarMenu)
        {
            dataGridView1.Rows.Clear();
            foreach (var menu in daftarMenu)
            {
                dataGridView1.Rows.Add(menu.IdMenu, menu.Nama, menu.Harga);
            }
        }

        public void SetTextFields(int idMenu, string namaMenu, int harga)
        {
            txtId.Text = idMenu.ToString();
            txtNama.Text = namaMenu;
            txtHarga.Text = harga.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        public void ClearText()
        {
            txtHarga.Clear();
            txtId.Clear();
            txtNama.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _controller.AddMenu(txtId.Text, txtNama.Text, txtHarga.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _controller.DeleteMenu(txtId.Text);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _controller.UpdateMenu(txtId.Text, txtNama.Text, txtHarga.Text);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
