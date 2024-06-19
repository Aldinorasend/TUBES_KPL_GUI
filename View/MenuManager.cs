using Main.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = Main.Model.Menu;

namespace View
{
    public partial class MenuManager : Form
    {
        public string fileDataPathMenu = "D:\\C# Project\\TUBES_GUI_RESTO_KPL_NEWEST\\Main\\Data\\dataMenu.json";

        public MenuManager()
        {
            InitializeComponent();
            PopulateDataGridView();
            this.dataGridView1.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_RowHeaderMouseClick);

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Mendapatkan nilai dari sel dalam baris yang diklik
                string IdMenu = row.Cells[0].Value?.ToString() ?? string.Empty;
                string namaMenu = row.Cells[1].Value?.ToString() ?? string.Empty;

                // Penanganan konversi Harga
                int Harga = 0;
                if (row.Cells[2].Value != null && int.TryParse(row.Cells[2].Value.ToString(), out int parsedHarga))
                {
                    Harga = parsedHarga;
                }

                // Menetapkan nilai ke TextBox
                txtId.Text = IdMenu;
                txtNama.Text = namaMenu;
                txtHarga.Text = Harga.ToString();

               
            }
        }

        private void PopulateDataGridView()
        {

            dataGridView1.Rows.Clear();
            List<Menu> daftarMenu = ReadJsonFile(fileDataPathMenu);

            foreach (var menu in daftarMenu)
            {
                dataGridView1.Rows.Add(menu.IdMenu, menu.Nama, menu.Harga);
            }
        }

        private List<Menu> ReadJsonFile(string fileDataPathMenu)
        {
            List<Menu> daftarMenu = new List<Menu>();

            try
            {
                if (string.IsNullOrEmpty(fileDataPathMenu))
                {
                    throw new ArgumentException("File path cannot be null or empty.", nameof(fileDataPathMenu));
                }

                string json;
                using (StreamReader reader = new StreamReader(fileDataPathMenu))
                {
                    json = reader.ReadToEnd();
                }

                daftarMenu = JsonSerializer.Deserialize<List<Menu>>(json);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}");
            }
            catch (System.Text.Json.JsonException ex)
            {
                MessageBox.Show($"Error parsing JSON file: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading JSON file: {ex.Message}");
            }

            return daftarMenu;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void ClearText()
        {
            txtHarga.Clear();
            txtId.Clear();
            txtNama.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            String idMenu = txtId.Text;
            String nama = txtNama.Text;
            int harga = int.Parse(txtHarga.Text);
            List<Menu> dtmenu = ReadJSON();
            bool statusMenu = Validate(idMenu);

            if (statusMenu)
            {
                MessageBox.Show("Id Menu telah digunakan");
            }
            else
            {
                Menu dtMn = new Menu(idMenu, nama, harga);
                dtmenu.Add(dtMn);
                WriteJSON(dtmenu);
                MessageBox.Show("Menu berhasil ditambahkan");

                // Refresh DataGridView
                PopulateDataGridView();
            }
        }

        private bool Validate(string idMenu)
        {
            List<Menu> dtMenu = ReadJSON();
            for (int i = 0; i < dtMenu.Count; i++)
            {
                if (dtMenu[i].IdMenu.Equals(idMenu))
                {
                    return true;
                }
            }
            return false;
        }

        private void WriteJSON(List<Menu> dtmenu)
        {

         string fileDataPathMenu = "D:\\C# Project\\TUBES_GUI_RESTO_KPL_NEWEST\\Main\\Data\\dataMenu.json";
        JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(dtmenu, options);
            File.WriteAllText(fileDataPathMenu, jsonString);
        }

        private List<Menu> ReadJSON()
        {

            string fileDataPathMenu = "D:\\C# Project\\TUBES_GUI_RESTO_KPL_NEWEST\\Main\\Data\\dataMenu.json";
            List<Menu> Data = new List<Menu>();
            try
            {
                string configjsonData = File.ReadAllText(fileDataPathMenu);
                Data = JsonSerializer.Deserialize<List<Menu>>(configjsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);


            }
            return Data;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<Menu> list = ReadJSON();
            string idMenu = txtId.Text;
            if (string.IsNullOrEmpty(idMenu))
            {
                MessageBox.Show("Masukkan Nama Barang");
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (idMenu.Equals(list[i].IdMenu))
                    {
                        list.Remove(list[i]);
                        WriteJSON(list);
                        MessageBox.Show("Menu terhapus!");
                        ClearText();
                        PopulateDataGridView();
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<Menu> list = ReadJSON();
            string idMenu = txtId.Text;
            string nama = txtNama.Text;
            int harga = int.Parse(txtHarga.Text);

            if (string.IsNullOrEmpty(idMenu))
            {
                MessageBox.Show("Masukkan Id Menu");
            }
            else
            {
                bool found = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (idMenu.Equals(list[i].IdMenu))
                    {
                        list[i].Nama = nama;
                        list[i].Harga = harga;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    WriteJSON(list);
                    MessageBox.Show("Menu berhasil diupdate!");
                    ClearText();
                    PopulateDataGridView();
                }
                else
                {
                    MessageBox.Show("Id Menu tidak ditemukan!");
                }
            }
        }
    }
}
