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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Иван
{
    public partial class регистрация : Form
    {
        DB db = new DB();
        public регистрация()
        {
            InitializeComponent();            
        }         
        public Boolean checkUser()
        {          
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `a` WHERE `login` = @uL ", db.GetConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = login.Text;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой логин уже есть");
                return true;
            }
            else
            {
                return false;
            }           
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            авторизация form1 = new авторизация();
            form1.Show();
            this.Hide();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            db.openConnection();
            if (name.Text == "")
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (surename.Text == "")
            {
                MessageBox.Show("Введите фамилию");
                return;
            }
            if (login.Text == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (pas.Text == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            if (checkUser())
            {
                return;
            }
            MySqlCommand command = new MySqlCommand("INSERT INTO `a` (`login`, `pas`, `name`, `surename`) VALUES (@login, @pas, @name, @surename)", db.GetConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login.Text;
            command.Parameters.Add("@pas", MySqlDbType.VarChar).Value = pas.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name.Text;
            command.Parameters.Add("@surename", MySqlDbType.VarChar).Value = surename.Text;  
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт создан");
            }
            else
            {
                MessageBox.Show("Аккаунт не создан");
            }     
            db.closeConnection();
        }
        private void регистрация_Load(object sender, EventArgs e)
        {

        }
    }
}
