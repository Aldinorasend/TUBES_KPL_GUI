﻿using Main.Model;
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
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";

            dataGridView1.DataSource = displayOrders;

            comboBox1.DataSource = dataMenu;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
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
                        orderToUpdate.Status = "Completed";
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
                string configJsonData = File.ReadAllText(filePathDataOrder);
                dataOrder = JsonSerializer.Deserialize<List<Pesanan>>(configJsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
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

            string jsonString = JsonSerializer.Serialize(newOrder, options);
            File.WriteAllText(filePathDataOrder, jsonString);
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
            public string MenuName { get; set; }
            public int Qty { get; set; }
            public int Total { get; set; }
            public string Status { get; set; }

            public DisplayOrder(string menuName, int qty, int total, string status)
            {
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
                    displayOrders.Add(new DisplayOrder(menu.Nama, order.Qty, total, order.Status));
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
    }
}