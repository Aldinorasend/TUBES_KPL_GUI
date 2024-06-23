using Main.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace View
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Event handler untuk klik tombol Register
        private void button2_Click(object sender, EventArgs e)
        {
            Register formRegister = new Register();
            formRegister.Show();
            this.Hide();
        }

        // Event handler untuk klik tombol Login
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username atau Password tidak boleh kosong!");
                return;
            }

            List<Akun> daftarAkun = BacaJSON();
            Akun akunTerdaftar = ValidasiPengguna(daftarAkun, username, password);

            if (akunTerdaftar != null)
            {
                Homepage halamanUtama = new Homepage();
                halamanUtama.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Akun anda tidak terdaftar");
            }
        }

        // Metode untuk validasi pengguna berdasarkan data akun
        private Akun ValidasiPengguna(List<Akun> daftarAkun, string username, string password)
        {
            foreach (Akun akun in daftarAkun)
            {
                if (akun.Username.Equals(username) && akun.Password.Equals(password))
                {
                    return akun;
                }
            }
            return null;
        }

        // Metode untuk membaca data akun dari file JSON
        public List<Akun> BacaJSON()
        {
            string filePath = Path.Combine(Application.StartupPath, "Data", "dataAkun.json");
            List<Akun> daftarAkun = new List<Akun>();

            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    daftarAkun = JsonSerializer.Deserialize<List<Akun>>(jsonData);
                }
                else
                {
                    MessageBox.Show("File data tidak ditemukan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kesalahan dalam membaca data: " + ex.Message);
            }

            return daftarAkun;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Tangani perubahan teks jika diperlukan
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Tangani perubahan teks jika diperlukan
        }
    }
}
