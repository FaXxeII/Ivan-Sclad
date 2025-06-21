using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Иван
{
    public partial class мейн : Form
    {
        public мейн()
        {
            InitializeComponent();
        }       
        private void button1_Click(object sender, EventArgs e)
        {
            склад form3 = new склад();
            form3.Show();
            this.Hide();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Персонал form6 = new Персонал();
            form6.Show();
            this.Hide();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            авторизация form3 = new авторизация();
            form3.Show();
            this.Hide();
        }
        private void мейн_Load(object sender, EventArgs e)
        {

        }
    }
}
