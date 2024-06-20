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
        private List<Menu> menus;
        private List<Pesanan> pesananList;

        public OrderManager()
        {
            InitializeComponent();
            LoadOrderData();
        }
        private void LoadOrderData()
        {
            pesananList = ReadJSON();
            menus = ReadMenuJSON();
            var displayOrders = CombineOrderWithMenu(pesananList, menus);

            dataGridView1.DataSource = displayOrders;
            comboBox1.DataSource = menus;
            comboBox1.DisplayMember = "Name";
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

    

      

      
        public List<Pesanan> ReadJSON()
        {
            string filePathDataOrder = Path.Combine(Application.StartupPath, "Data", "dataPesanan.json");
            List<Pesanan> dataOrder = new List<Pesanan>();
            try
            {
                if (File.Exists(filePathDataOrder))
                {
                    string configJsonData = File.ReadAllText(filePathDataOrder);
                    dataOrder = JsonSerializer.Deserialize<List<Pesanan>>(configJsonData);
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
        private void WriteJSON(List<Pesanan> newOrder)
        {
            string filePathDataOrder = Path.Combine(Application.StartupPath, "Data", "dataPesanan.json");
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            try
            {
                string jsonString = JsonSerializer.Serialize(newOrder, options);
                File.WriteAllText(filePathDataOrder, jsonString);

                LoadOrderData(); // Memuat ulang data setelah menyimpan ke file JSON
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing JSON file: " + e.Message);
            }
        }
        public List<Menu> ReadMenuJSON()
        {
            string filePathDataMenu = Path.Combine(Application.StartupPath, "Data", "dataMenu.json");
            List<Menu> dataMenu = new List<Menu>();
            try
            {
                if (File.Exists(filePathDataMenu))
                {
                    string configJsonData = File.ReadAllText(filePathDataMenu);
                    dataMenu = JsonSerializer.Deserialize<List<Menu>>(configJsonData);
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
        public class DisplayOrder
        {
            public string OrderName { get; set; }
            public int IdMenu { get; set; }
            public string MenuName { get; set; }
            public int HargaMenu { get; set; }
            public int Qty { get; set; }
            public int Total { get; set; }
            public string Status { get; set; }

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

        private List<DisplayOrder> CombineOrderWithMenu(List<Pesanan> orders, List<Menu> menus)
        {
            var displayOrders = new List<DisplayOrder>();

            foreach (var order in orders)
            {
                var menu = menus.FirstOrDefault(m => m.IdMenu == order.menuId);
                if (menu != null)
                {
                    int total = menu.Harga * order.Qty;
                    displayOrders.Add(new DisplayOrder(order.Name, menu.IdMenu, menu.Harga, menu.Nama, order.Qty, total, order.Status));
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
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
                MessageBox.Show("Masukkan tidak mencukupi.");
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            Close();
        }
    }
}
