using Main.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Menu = Main.Model.Menu;

namespace View
{
    public partial class OrderState : Form
    {
        private List<Menu> _menuList;
        private List<Pesanan> _orderList;

        public OrderState()
        {
            InitializeComponent();
            PopulateDataGrid();
        }

        // Mengisi DataGridView dengan data menu dan pesanan yang digabungkan
        private void PopulateDataGrid()
        {
            _orderList = ReadJSON();
            _menuList = ReadMenuJSON();
            var displayOrders = CombineOrderWithMenu(_orderList, _menuList);

            dataGridView1.DataSource = displayOrders;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        // Event handler untuk menyelesaikan pesanan
        private void buttonComplete_Click(object sender, EventArgs e)
        {
            UpdateOrderStatus("Completed");
        }

        // Event handler untuk membatalkan pesanan
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            UpdateOrderStatus("Cancelled");
        }

        // Event handler untuk kembali ke tampilan sebelumnya
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Memperbarui status pesanan yang dipilih
        private void UpdateOrderStatus(string newStatus)
        {
            // Mengecek apakah ada baris yang dipilih di dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Iterasi melalui semua baris yang dipilih
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // Mengambil ID menu dari sel "IdMenu" pada baris yang dipilih
                    int menuId = Convert.ToInt32(row.Cells["IdMenu"].Value);

                    // Mencari pesanan dalam daftar pesanan (_orderList) yang memiliki menuId yang sesuai dan status "Pending"
                    var orderToUpdate = _orderList.FirstOrDefault(p => p.menuId == menuId && p.Status == "Pending");

                    // Jika pesanan ditemukan dan statusnya adalah "Pending"
                    if (orderToUpdate != null)
                    {
                        // Memperbarui status pesanan menjadi newStatus
                        orderToUpdate.Status = newStatus;
                    }
                    else
                    {
                        MessageBox.Show($"Pesanan dengan ID Menu {menuId} tidak ditemukan atau tidak Pending.");
                    }
                }
                // Menyimpan daftar pesanan yang telah diperbarui ke file JSON
                WriteJSON(_orderList);

                // Memperbarui tampilan data pada DataGridView dengan reload data
                PopulateDataGrid();
            }
            else
            {
                MessageBox.Show("Silakan pilih pesanan untuk diperbarui.");
            }
        }

        // Membaca pesanan dari file JSON
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
                    Console.WriteLine($"File '{filePathDataOrder}' tidak ditemukan.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error membaca file JSON: " + e.Message);
            }

            return dataOrder;
        }

        // Menulis pesanan ke file JSON
        private void WriteJSON(List<Pesanan> newOrder)
        {
            string filePathDataOrder = Path.Combine(Application.StartupPath, "Data", "dataPesanan.json");
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            try
            {
                string jsonString = JsonSerializer.Serialize(newOrder, options);
                File.WriteAllText(filePathDataOrder, jsonString);
                PopulateDataGrid(); // Reload data setelah menyimpan ke file JSON
            }
            catch (Exception e)
            {
                Console.WriteLine("Error menulis file JSON: " + e.Message);
            }
        }

        // Membaca menu dari file JSON
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
                Console.WriteLine("Error membaca file JSON: " + e.Message);
            }

            return dataMenu;
        }

        // Menggabungkan pesanan dengan item menu untuk ditampilkan
        private List<DisplayOrder> CombineOrderWithMenu(List<Pesanan> orders, List<Menu> menus)
        {
            var displayOrders = new List<DisplayOrder>();

            foreach (var order in orders)
            {
                var menu = menus.FirstOrDefault(m => m.IdMenu == order.menuId);
                if (menu != null)
                {
                    int total = menu.Harga * order.Qty;
                    displayOrders.Add(new DisplayOrder(menu.IdMenu, order.Name, menu.Nama, order.Qty, total, order.Status));
                }
            }

            return displayOrders;
        }

        // Kelas untuk merepresentasikan data pesanan dan menu yang digabungkan untuk ditampilkan
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
    }
}
