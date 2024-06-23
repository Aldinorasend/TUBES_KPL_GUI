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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class AkunManager : Form
    {
        // Path file data akun JSON
        public String fileDataPathAkun = Path.Combine(System.Windows.Forms.Application.StartupPath, "Data", "dataAkun.json");

        public AkunManager()
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
                string Username = row.Cells[0].Value?.ToString() ?? string.Empty;
                string Password = row.Cells[1].Value?.ToString() ?? string.Empty;
                string Nama = row.Cells[2].Value?.ToString() ?? string.Empty;

                Akun akn = new Akun(SanitizeInput(Username), SanitizeInput(Password), SanitizeInput(Nama));
                txtUname.Text = akn.Username;
                txtPassword.Text = akn.Password;
                txtName.Text = akn.Nama;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kosongkan metode ini jika tidak ada logika yang ingin ditambahkan saat sel diklik
        }

        private void PopulateDataGridView()
        {
            dataGridView1.Rows.Clear();
            List<Akun> daftarAkun = ReadJsonFile(fileDataPathAkun);

            foreach (var Akun in daftarAkun)
            {
                dataGridView1.Rows.Add(Akun.Username, Akun.Password, Akun.Nama);
            }
        }

        private List<Akun> ReadJsonFile(string filePath)
        {
            List<Akun> daftarAkun = new List<Akun>();

            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
                }

                string json;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    json = reader.ReadToEnd();
                }

                daftarAkun = JsonSerializer.Deserialize<List<Akun>>(json);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}");
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing JSON file: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading JSON file: {ex.Message}");
            }

            return daftarAkun;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Membersihkan text box
            txtUname.Clear();
            txtPassword.Clear();
            txtName.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string usernameToDelete = selectedRow.Cells[0].Value?.ToString() ?? string.Empty;

                // Validasi input
                if (string.IsNullOrWhiteSpace(usernameToDelete))
                {
                    MessageBox.Show("Invalid username.");
                    return;
                }

                // Membaca daftar akun saat ini dari file JSON
                List<Akun> daftarAkun = ReadJsonFile(fileDataPathAkun);

                // Menghapus akun dengan username yang dipilih
                daftarAkun.RemoveAll(a => a.Username == SanitizeInput(usernameToDelete));

                // Menyimpan daftar akun yang diperbarui kembali ke file JSON
                string updatedJson = JsonSerializer.Serialize(daftarAkun);
                File.WriteAllText(fileDataPathAkun, updatedJson);

                // Menyegarkan DataGridView
                PopulateDataGridView();

                // Membersihkan text box
                btnClear_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Menyembunyikan form saat tombol kembali ditekan
            this.Hide();
        }

        // Method to sanitize input to prevent injection attacks
        private string SanitizeInput(string input)
        {
            if (input == null)
                return string.Empty;

            // Menghapus karakter yang tidak diinginkan
            return Regex.Replace(input, @"[^\w\s@.-]", string.Empty);
        }
    }
}
