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

      public Pesanan(int menuId, string Status, string Name, int Qty, int Jumlah)
        {
            this.menuId = menuId;
            this.Status = Status;
            this.Name = Name;
            this.Qty = Qty;
            this.Jumlah = Jumlah;
        }

    }
}
