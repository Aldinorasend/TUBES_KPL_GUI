using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class Homepage : Form
    {
        public Homepage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuManager mn = new MenuManager();
            mn.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AkunManager ak = new AkunManager();
            ak.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OrderState os = new OrderState();
            os.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OrderManager on = new OrderManager();
            on.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
