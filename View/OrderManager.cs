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
            var dataMenu = ReadMenuJSON();
            var displayOrders = CombineOrderWithMenu(pesananList, dataMenu);

            dataGridView1.DataSource = displayOrders;

            comboBox1.DataSource = dataMenu;
            comboBox1.DisplayMember = "Nama";
            comboBox1.ValueMember = "IdMenu";

            /* dataGridView1.Rows.Clear();
             List<Pesanan> daftarPesanan = ReadJSON();

             foreach (var pesanan in daftarPesanan)
             {
                 dataGridView1.Rows.Add(pesanan.Name, pesanan.Jumlah, pesanan.Qty, pesanan.Status, pesanan.menuId);
             }*/

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

     

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int idMenu = Convert.ToInt32(row.Cells["IdMenu"].Value); // Ambil ID Menu dari baris yang dipilih
                    var orderToUpdate = pesananList.FirstOrDefault(p => p.menuId == idMenu && p.Status == "Pending");

                    if (orderToUpdate != null)
                    {
                        orderToUpdate.Status = "Completed";
                    }
                    else
                    {
                        MessageBox.Show($"Order with ID Menu {idMenu} not found or is not Pending.");
                    }
                }
                WriteJSON(pesananList);
                LoadOrderData(); // Refresh DataGridView
            }
            else
            {
                MessageBox.Show("Please select an order to complete.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    string menuName = row.Cells["MenuName"].Value.ToString();
                    var orderToUpdate = pesananList.FirstOrDefault(p =>
                        menus.Any(m => m.IdMenu == p.menuId && m.Nama == menuName) && p.Status == "Pending");

                    if (orderToUpdate != null)
                    {
                        orderToUpdate.Status = "Cancelled";
                    }
                }
                WriteJSON(pesananList);
                LoadOrderData(); // Refresh DataGridView
            }
            else
            {
                MessageBox.Show("Please select an order to cancel.");
            }
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
                string configJsonData = File.ReadAllText(filePathDataMenu);
                dataMenu = JsonSerializer.Deserialize<List<Menu>>(configJsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return dataMenu;
        }
        public class DisplayOrder
        {
            public string OrderName { get; set; }
            public int IdMenu{ get; set; }
            public string MenuName { get; set; }
            public int HargaMenu { get; set; }
            public int Qty { get; set; }
            public int Total { get; set; }
            public string Status { get; set; }

            public DisplayOrder(string orderName,int idMenu,int hargaMenu, string menuName,int qty, int total, string status)
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
                MessageBox.Show("Please fill all the fields correctly.");
            }
        }
    }
}
