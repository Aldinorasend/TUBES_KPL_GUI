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
    public partial class OrderState : Form
    {
        private List<Menu> xMenu;
        private List<Pesanan> xListOrder;
        public OrderState()
        {
            InitializeComponent();
            populateDataGrid();
        }
        private void populateDataGrid()
        {
            xListOrder = ReadJSON();
            var dataMenu = ReadMenuJSON();
            var displayOrders = CombineOrderWithMenu(xListOrder, dataMenu);

            dataGridView1.DataSource = displayOrders;


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
                    var orderToUpdate = xListOrder.FirstOrDefault(p => p.menuId == idMenu && p.Status == "Pending");

                    if (orderToUpdate != null)
                    {
                        orderToUpdate.Status = "Completed";
                    }
                    else
                    {
                        MessageBox.Show($"Order with ID Menu {idMenu} not found or is not Pending.");
                    }
                }
                WriteJSON(xListOrder);
                populateDataGrid(); // Refresh DataGridView
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
                    int idMenu = Convert.ToInt32(row.Cells["IdMenu"].Value); // Ambil ID Menu dari baris yang dipilih
                    var orderToUpdate = xListOrder.FirstOrDefault(p => p.menuId == idMenu && p.Status == "Pending");

                    if (orderToUpdate != null)
                    {
                        orderToUpdate.Status = "Cancelled";
                    }
                    else
                    {
                        MessageBox.Show($"Order with ID Menu {idMenu} not found or is not Pending.");
                    }
                }
                WriteJSON(xListOrder);
                populateDataGrid(); // Refresh DataGridView
            }
            else
            {
                MessageBox.Show("Please select an order to complete.");
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

                populateDataGrid(); // Memuat ulang data setelah menyimpan ke file JSON
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
            public int IdMenu { get; set; }
            public string NamaPelanggan { get; set; }
            public string MenuName { get; set; }
            public int Qty { get; set; }
            public int Total { get; set; }
            public string Status { get; set; }


            public DisplayOrder(int idMenu, string namaPelanggan, string menuName, int qty, int total, string status)
            {
                IdMenu = idMenu;
                NamaPelanggan = namaPelanggan;
                MenuName = menuName;
                Qty = qty;
                Total = total;
                Status = status;

            }
        }

        private List<DisplayOrder> CombineOrderWithMenu(List<Pesanan> orders, List<Menu> xMenu)
        {
            var displayOrders = new List<DisplayOrder>();

            foreach (var order in orders)
            {
                var menu = xMenu.FirstOrDefault(m => m.IdMenu == order.menuId);
                if (menu != null)
                {
                    int total = menu.Harga * order.Qty;
                    displayOrders.Add(new DisplayOrder(menu.IdMenu, order.Name, menu.Nama, order.Qty, total, order.Status));
                }
            }

            return displayOrders;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
