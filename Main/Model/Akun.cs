using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class Akun
    {
        public string Nama { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Akun(string Username, string Password, string Nama)
        {

            this.Username = Username;
            this.Password = Password;
            this.Nama = Nama;
        }
    }
}
