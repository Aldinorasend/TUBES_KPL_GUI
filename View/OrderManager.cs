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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Menu = Main.Model.Menu;

namespace View
{
    public partial class OrderManager : Form
    {
        private List<Menu> menus; // List untuk menyimpan data menu
        private List<Pesanan> pesananList; // List untuk menyimpan data pesanan

        // Konstruktor untuk inisialisasi form dan memuat data pesanan dan menu
        public OrderManager()
        {
            InitializeComponent();
            LoadOrderData();
        }

        // Method untuk memuat data pesanan dan menu, serta menggabungkannya untuk ditampilkan
        private void LoadOrderData()
        {
            pesananList = ReadJSON();
            menus = ReadMenuJSON();
            var displayOrders = CombineOrderWithMenu(pesananList, menus); // Menggabungkan data pesanan dengan data menu

            // Menampilkan data gabungan ke dalam DataGridView
            dataGridView1.DataSource = displayOrders;
            // Menampilkan data menu ke dalam ComboBox
            comboBox1.DataSource = menus;
            comboBox1.DisplayMember = "Nama";
            comboBox1.ValueMember = "IdMenu";

           

        }

        

        

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedMenu = (Menu)comboBox1.SelectedItem;
            var qty = (int)numericUpDown1.Value;
            var namaPelanggan = textBox1.Text;

            if (selectedMenu != null && qty > 0 && !string.IsNullOrWhiteSpace(namaPelanggan))
            {
                var total = selectedMenu.Harga * qty;
                var newOrder = new Pesanan(selectedMenu.IdMenu, "Pending", namaPelanggan, qty, total);
                pesananList.Add(newOrder);
                WriteJSON(pesananList);

                LoadOrderData(); // Refresh DataGridView
            }
            else
            {
                MessageBox.Show("Please fill all the fields correctly.");
            }
        }

       

     

        

        

       
     

       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Method untuk membaca data pesanan dari file JSON    




        public List<Pesanan> ReadJSON()
        {
            string filePathDataOrder = Path.Combine(Application.StartupPath, "Data", "dataPesanan.json");
            List<Pesanan> dataOrder = new List<Pesanan>();
            try
            {
                if (File.Exists(filePathDataOrder))
                {
                    string configJsonData = File.ReadAllText(filePathDataOrder);
                    dataOrder = JsonSerializer.Deserialize<List<Pesanan>>(configJsonData); // Deserialize JSON ke List Pesanan
                }
                else
                {
                    Console.WriteLine($"File '{filePathDataOrder}' not found.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading JSON file: " + e.Message);
            }
            return dataOrder;
        }

        // Method untuk menulis data pesanan baru ke file JSON
        private void WriteJSON(List<Pesanan> newOrder)
        {
            string filePathDataOrder = Path.Combine(Application.StartupPath, "Data", "dataPesanan.json");
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true // Format JSON dengan indentasi
            };
            try
            {
                string jsonString = JsonSerializer.Serialize(newOrder, options); // Serialize List Pesanan ke JSON
                File.WriteAllText(filePathDataOrder, jsonString); // Menulis JSON ke file

                LoadOrderData(); // Memuat ulang data setelah menyimpan ke file JSON
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing JSON file: " + e.Message);
            }
        }

        // Method untuk membaca data menu dari file JSON
        public List<Menu> ReadMenuJSON()
        {
            string filePathDataMenu = Path.Combine(Application.StartupPath, "Data", "dataMenu.json");
            List<Menu> dataMenu = new List<Menu>();
            try
            {
                if (File.Exists(filePathDataMenu))
                {
                    string configJsonData = File.ReadAllText(filePathDataMenu);
                    dataMenu = JsonSerializer.Deserialize<List<Menu>>(configJsonData); // Deserialize JSON ke List Menu
                }
                else
                {
                    Console.WriteLine($"File '{filePathDataMenu}' not found.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading JSON file: " + e.Message);
            }
            return dataMenu;
        }

        // Class untuk menampilkan data pesanan yang digabungkan dengan data menu
        public class DisplayOrder
        {
            public string OrderName { get; set; }
            public int IdMenu { get; set; }
            public string MenuName { get; set; }
            public int HargaMenu { get; set; }
            public int Qty { get; set; }
            public int Total { get; set; }
            public string Status { get; set; }

            // Konstruktor untuk inisialisasi DisplayOrder
            public DisplayOrder(string orderName, int idMenu, int hargaMenu, string menuName, int qty, int total, string status)
            {
                OrderName = orderName;
                IdMenu = idMenu;
                HargaMenu = hargaMenu;
                MenuName = menuName;
                Qty = qty;
                Total = total;
                Status = status;
            }
        }

        // Method untuk menggabungkan data pesanan dengan data menu dan menghasilkan list DisplayOrder
        private List<DisplayOrder> CombineOrderWithMenu(List<Pesanan> orders, List<Menu> menus)
        {
            var displayOrders = new List<DisplayOrder>();

            foreach (var order in orders)
            {
                var menu = menus.FirstOrDefault(m => m.IdMenu == order.menuId); // Mencari menu berdasarkan IdMenu
                if (menu != null)
                {
                    int total = menu.Harga * order.Qty; // Menghitung total harga
                    displayOrders.Add(new DisplayOrder(order.Name, menu.IdMenu, menu.Harga, menu.Nama, order.Qty, total, order.Status)); // Menambahkan data gabungan ke list
                }
            }

            return displayOrders;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Event handler untuk klik tombol Close, menutup form
        private void button2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        // Event handler untuk klik tombol Add Order, menambahkan pesanan baru
        private void button1_Click_1(object sender, EventArgs e)
        {
            var selectedMenu = (Menu)comboBox1.SelectedItem;
            var qty = (int)numericUpDown1.Value; 
            var namaPelanggan = textBox1.Text; 

            // Validasi input
            if (selectedMenu != null && qty > 0 && !string.IsNullOrWhiteSpace(namaPelanggan))
            {
                var total = selectedMenu.Harga * qty; 
                var newOrder = new Pesanan(selectedMenu.IdMenu, "Pending", namaPelanggan, qty, total); 
                pesananList.Add(newOrder); 
                WriteJSON(pesananList); 

                LoadOrderData(); // Memuat ulang data ke DataGridView
            }
            else
            {
                MessageBox.Show("Masukkan tidak mencukupi."); // Menampilkan pesan jika input tidak valid
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            Close();
        }
    }
}
