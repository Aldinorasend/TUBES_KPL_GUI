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

namespace View
{
    public partial class Register : Form
    {
        // Constructor to initialize the form
        public Register()
        {
            InitializeComponent();
        }

        // Event handler for the Cancel button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Event handler for the Register button
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string nama = textBox1.Text.Trim();

            // Read existing account data
            List<Akun> dataAkun = ReadJSON();

            // Validate input fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong.");
                return;
            }

            // Check if the user already exists
            bool akunDitemukan = ValidateUser(dataAkun, username);
            if (akunDitemukan)
            {
                MessageBox.Show("Akun sudah ada.");
                return;
            }

            // Validate password strength
            bool statusPassword = ValidatePassword(password);
            if (!statusPassword)
            {
                MessageBox.Show("Password harus berisi lebih dari 6 dan kurang dari 10 karakter.");
                return;
            }

            // Create and add the new account
            Akun newAkun = new Akun(username, password, nama);
            dataAkun.Add(newAkun);
            WriteJSON(dataAkun);

            MessageBox.Show("Akun berhasil dibuat.");
            this.Dispose();

            // Show the login form
            Login login = new Login();
            login.Show();
        }

        // Method to write the account data to a JSON file
        private void WriteJSON(List<Akun> dataAkun)
        {
            string filePathDataAkun = Path.Combine(Application.StartupPath, "Data", "dataAkun.json");
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

            string jsonString = JsonSerializer.Serialize(dataAkun, options);
            File.WriteAllText(filePathDataAkun, jsonString);
        }

        // Method to validate password strength
        private bool ValidatePassword(string password)
        {
            return password.Length >= 6 && password.Length <= 10;
        }

        // Method to check if a user already exists
        private bool ValidateUser(List<Akun> dataAkun, string username)
        {
            return dataAkun.Any(akun => akun.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // Method to read the account data from a JSON file
        public List<Akun> ReadJSON()
        {
            string filePathDataAkun = Path.Combine(Application.StartupPath, "Data", "dataAkun.json");
            List<Akun> dataAkun = new List<Akun>();

            try
            {
                string configJsonData = File.ReadAllText(filePathDataAkun);
                dataAkun = JsonSerializer.Deserialize<List<Akun>>(configJsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return dataAkun;
        }

        // Event handler for form load
        private void Register_Load(object sender, EventArgs e)
        {
            // Add any initialization code here if needed
        }
    }
}
