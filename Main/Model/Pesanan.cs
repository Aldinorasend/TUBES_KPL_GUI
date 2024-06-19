using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class Pesanan
    {
        public int menuId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public int Jumlah { get; set; }

        public Pesanan(int _menuId, string _Status, string _Name, int _Qty, int _Jumlah)
        {
            this.menuId = _menuId;
            this.Status = _Status;
            this.Name = _Name;
            this.Qty = _Qty;
            this.Jumlah = _Jumlah;
        }
    }
}
