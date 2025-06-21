using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Management;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Иван
{
    public partial class авторизация : Form
    {   
        DB db = new DB();
       
        public авторизация()
        {
            InitializeComponent();           
        }          
        private void button1_Click(object sender, EventArgs e)
        {
            String LoginUser = username.Text;
            String PassUser = password.Text;            
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `a` WHERE `login` = @uL AND `pas` = @uP", db.GetConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = LoginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = PassUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                мейн form5 = new мейн();
                form5.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Такого пользователя нет");
        }    
        private void button2_Click(object sender, EventArgs e)
        {
            регистрация form2 = new регистрация();
            form2.Show();
            this.Hide();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }            
        private void label4_Click_1(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }
        private void авторизация_Load(object sender, EventArgs e)
        {

        }      
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (password.UseSystemPasswordChar)
            {
                password.UseSystemPasswordChar = false;               
            }
            else
            {
                password.UseSystemPasswordChar = true;             
            }
        }
    }
}