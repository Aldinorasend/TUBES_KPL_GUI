using Main.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using View;
using static System.Net.Mime.MediaTypeNames;
using Menu = Main.Model.Menu;

namespace Controller
{
    public class MenuController
    {
        private readonly MenuManager _view;
        private readonly string _fileDataPathMenu;

        /*Pendefinisian path dari json*/
        public MenuController(MenuManager view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _fileDataPathMenu = Path.Combine(System.Windows.Forms.Application.StartupPath, "Data", "dataMenu.json");
        }

        /*Selection Row*/
        public void RowHeaderMouseClick(int rowIndex)
        {
            if (rowIndex < 0) return;

            DataGridViewRow row = _view.dataGridView1.Rows[rowIndex];
            if (row.Cells[0].Value == null || !int.TryParse(row.Cells[0].Value.ToString(), out int idMenu)) return;

            string namaMenu = row.Cells[1].Value?.ToString() ?? string.Empty;
            if (row.Cells[2].Value != null && int.TryParse(row.Cells[2].Value.ToString(), out int harga))
            {
                _view.SetTextFields(idMenu, namaMenu, harga);
            }
        }

        /*Baca json dan Tampil ke DataGridView*/
        public void PopulateDataGridView()
        {
            List<Menu> daftarMenu = ReadMenuData();
            _view.PopulateDataGridView(daftarMenu);
        }

        /*Fungsionalitas CRUD Menu*/
        public void AddMenu(string idText, string nama, string hargaText)
        {
            if (!int.TryParse(idText, out int idMenu) || !int.TryParse(hargaText, out int harga))
            {
                MessageBox.Show("ID dan Harga Harus berupa Angka.");
                return;
            }

            List<Menu> menuList = ReadMenuData();
            if (IsMenuIdExists(idMenu))
            {
                MessageBox.Show("Id Menu telah digunakan");
            }
            else
            {
                Menu newMenu = new Menu(idMenu, nama, harga);
                menuList.Add(newMenu);
                WriteMenuData(menuList);
                MessageBox.Show("Menu berhasil ditambahkan");
                _view.ClearText();
                PopulateDataGridView();
            }
        }

        public void DeleteMenu(string idText)
        {
            if (!int.TryParse(idText, out int idMenu))
            {
                MessageBox.Show("Invalid Id Menu");
                return;
            }

            List<Menu> menuList = ReadMenuData();
            Menu menuToRemove = menuList.Find(menu => menu.IdMenu == idMenu);

            if (menuToRemove != null)
            {
                menuList.Remove(menuToRemove);
                WriteMenuData(menuList);
                MessageBox.Show("Menu terhapus!");
                _view.ClearText();
                PopulateDataGridView();
            }
            else
            {
                MessageBox.Show("Menu tidak ditemukan!");
            }
        }

        public void UpdateMenu(string idText, string nama, string hargaText)
        {
            if (!int.TryParse(idText, out int idMenu) || !int.TryParse(hargaText, out int harga))
            {
                MessageBox.Show("ID dan Harga harus berupa angka.");
                return;
            }

            List<Menu> menuList = ReadMenuData();
            Menu menuToUpdate = menuList.Find(menu => menu.IdMenu == idMenu);

            if (menuToUpdate != null)
            {
                menuToUpdate.Nama = nama;
                menuToUpdate.Harga = harga;
                WriteMenuData(menuList);
                MessageBox.Show("Menu berhasil diupdate!");
                _view.ClearText();
                PopulateDataGridView();
            }
            else
            {
                MessageBox.Show("Menu not tidak ditemukan!");
            }
        }

        private List<Menu> ReadMenuData()
        {
            try
            {
                if (!File.Exists(_fileDataPathMenu)) return new List<Menu>();

                string jsonData = File.ReadAllText(_fileDataPathMenu);
                return JsonSerializer.Deserialize<List<Menu>>(jsonData) ?? new List<Menu>();
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
            return new List<Menu>();
        }

        private void WriteMenuData(List<Menu> menuList)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(menuList, options);
                File.WriteAllText(_fileDataPathMenu, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing JSON file: {ex.Message}");
            }
        }

        /*Pengecekan IdMenu*/
        private bool IsMenuIdExists(int idMenu)
        {
            List<Menu> menuList = ReadMenuData();
            return menuList.Exists(menu => menu.IdMenu == idMenu);
        }
    }
}
