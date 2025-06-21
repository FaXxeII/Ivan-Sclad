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
    public partial class Персонал : Form
    {
        DB db = new DB();
        public Персонал()
        {
            InitializeComponent();
        }
        private void Персонал_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            мейн form5 = new мейн();
            form5.Show();
            this.Hide();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
        }
        private void LoadData()
        {
            db.openConnection();
            try
            {      
                string query = "SELECT * FROM `pers`";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, db.GetConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }   
            db.closeConnection();
        }      
        private void button4_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(searchText)) return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string position = row.Cells["Должность"].Value?.ToString();
                string fullName = row.Cells["ФИО"].Value?.ToString();
                string number = row.Cells["Номер"].Value?.ToString();

                bool isMatch = string.Equals(position, searchText, StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(fullName, searchText, StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(number, searchText, StringComparison.OrdinalIgnoreCase);

                if (isMatch)
                {
                    row.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }

            MessageBox.Show("Не найдено");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            db.openConnection();
            try
            {
                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    if (cell.IsInEditMode || cell.OwningRow.IsNewRow)
                        continue;
                    var row = cell.OwningRow;
                    var id = row.Cells["id"].Value;
                    var dol = row.Cells["Должность"].Value;
                    var name = row.Cells["ФИО"].Value;
                    var nom = row.Cells["Номер"].Value;
                    string query = "UPDATE pers SET Должность=@dol, ФИО=@fio, Номер=@nom WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dol", dol);
                    cmd.Parameters.AddWithValue("@fio", name);
                    cmd.Parameters.AddWithValue("@nom", nom);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Сохранено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            db.closeConnection();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            db.openConnection();
            if (dataGridView1.SelectedRows.Count > 0 && !dataGridView1.SelectedRows[0].IsNewRow)
            {             
                try
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0]; 
                    string dol = selectedRow.Cells["Должность"].Value.ToString();
                    string name = selectedRow.Cells["ФИО"].Value.ToString();
                    string nom = selectedRow.Cells["Номер"].Value.ToString();                                                                              
                    string query = "DELETE FROM pers WHERE Должность = @dol AND ФИО = @name AND Номер = @nom";
                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                    cmd.Parameters.AddWithValue("@dol", dol);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@nom", nom);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Строка успешно удалена из базы данных!");
                        dataGridView1.Rows.RemoveAt(selectedRow.Index);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить строку из базы данных.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении строки" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления");
            }
            db.closeConnection();
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            db.openConnection();
            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите должность");
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите ФИО");
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите номер");
                return;
            }
            MySqlCommand command = new MySqlCommand("INSERT INTO `pers` (`Должность`, `ФИО`, `Номер`) VALUES (@dol, @name, @nom)", db.GetConnection());
            command.Parameters.Add("@dol", MySqlDbType.VarChar).Value = textBox2.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = textBox3.Text;
            command.Parameters.Add("@nom", MySqlDbType.VarChar).Value = textBox4.Text;
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Успешно добавлено!");
                LoadData();
            }
            else
            {
                MessageBox.Show("Не добавлено");
            }       
            db.closeConnection();           
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }        
    }
}
