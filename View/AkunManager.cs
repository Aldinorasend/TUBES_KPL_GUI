using System;
using System.Collections.Generic;

using System.IO;

using System.Text.Json;

using System.Windows.Forms;
using Main.Model;

namespace View
{
    public partial class AkunManager : Form
    {
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
                string Username = row.Cells[0].Value.ToString();
                string Password = row.Cells[1].Value.ToString();
                string Nama = row.Cells[2].Value.ToString();


                Akun akn = new Akun(Username, Password, Nama);
                txtUname.Text = akn.Username;
                txtPassword.Text = akn.Password;
                txtName.Text = akn.Nama;


            }

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUname.Clear();
            txtPassword.Clear();
            txtName.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string usernameToDelete = selectedRow.Cells[0].Value.ToString();

                // Read the current list of Akun from the JSON file
                List<Akun> daftarAkun = ReadJsonFile(fileDataPathAkun);

                // Remove the Akun with the selected username
                daftarAkun.RemoveAll(a => a.Username == usernameToDelete);

                // Serialize the updated list back to the JSON file
                string updatedJson = JsonSerializer.Serialize(daftarAkun);
                File.WriteAllText(fileDataPathAkun, updatedJson);

                // Refresh the DataGridView
                PopulateDataGridView();

                // Clear the text boxes
                btnClear_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
