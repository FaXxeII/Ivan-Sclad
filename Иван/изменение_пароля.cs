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

namespace Иван
{
    public partial class Form4 : Form
    { 
        DB db = new DB();
        public Form4()
        {
            InitializeComponent();           
        }     
        private void button1_Click(object sender, EventArgs e)
        {
            авторизация form1 = new авторизация();
            form1.Show();
            this.Hide();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            db.openConnection();
            string login = textBox1.Text.Trim();
            string newPass = textBox2.Text;
            string confirmPass = textBox3.Text;
            if (string.IsNullOrEmpty(login)  || string.IsNullOrEmpty(newPass)  || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }
            if (newPass != confirmPass)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }
            try
            {                                                                       
                string check = "SELECT COUNT(*) FROM a WHERE login = @login";
                MySqlCommand command = new MySqlCommand(check, db.GetConnection());
                command.Parameters.AddWithValue("@login", login);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    MessageBox.Show("Такого логина нет.");
                    return;
                }
                string update = "UPDATE a SET pas = @pas WHERE login = @login";
                MySqlCommand updateCmd = new MySqlCommand(update, db.GetConnection());
                updateCmd.Parameters.AddWithValue("@pas", newPass);
                updateCmd.Parameters.AddWithValue("@login", login);

                int rows = updateCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Пароль успешно изменён.");
                }
                else
                {
                    MessageBox.Show("Ошибка при изменении пароля.");
                }                                                         
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            db.closeConnection();
        }
        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
