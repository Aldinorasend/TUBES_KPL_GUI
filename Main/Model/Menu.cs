using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class Menu
    {
        public int IdMenu { get; set; }

        public string Nama { get; set; }

        public int Harga { get; set; }



        public Menu(int IdMenu, string Nama, int Harga)
        {

            this.IdMenu = IdMenu;
            this.Nama = Nama;
            this.Harga = Harga;

        }
    }
}
