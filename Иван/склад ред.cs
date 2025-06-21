using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Иван
{
    public partial class Ред_склад : Form
    {
        DB db = new DB();
        public Ред_склад()
        {
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            склад form3 = new склад();
            form3.Show();
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
                string query = "SELECT * FROM `osnova`";
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
        private void button2_Click(object sender, EventArgs e)
        {
            db.openConnection();
            if (comboBox2.Text == "Тип")
            {
                MessageBox.Show("Выберете тип");
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите название");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите колличество");
                return;
            }
            MySqlCommand command = new MySqlCommand("INSERT INTO `osnova` (`Вид`, `Название`, `Количество`) VALUES (@vid, @name, @kol)", db.GetConnection());
            command.Parameters.Add("@vid", MySqlDbType.VarChar).Value = comboBox2.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@kol", MySqlDbType.VarChar).Value = textBox2.Text;            
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
            textBox1.Clear();
            textBox2.Clear();         
        }
        private void button3_Click(object sender, EventArgs e)
        {
            db.openConnection();
            if (dataGridView1.SelectedRows.Count > 0 && !dataGridView1.SelectedRows[0].IsNewRow)
            {
                try
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string name = selectedRow.Cells["Название"].Value.ToString();
                    string kol = selectedRow.Cells["Количество"].Value.ToString();                   
                    string query = "DELETE FROM osnova WHERE Название = @name AND Количество = @kol";
                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@kol", kol);
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
        private void button4_Click(object sender, EventArgs e)
        {
            string searchText = textBox3.Text.Trim();
            string domainSearch = comboBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchText)) searchText = null;
            if (string.IsNullOrEmpty(domainSearch)) domainSearch = null;

            if (searchText == null && domainSearch == null) return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                bool matchesText = searchText == null ||
                                  row.Cells["Название"].Value?.ToString().Equals(searchText, StringComparison.OrdinalIgnoreCase) == true;

                bool matchesDomain = domainSearch == null ||
                                    row.Cells["Вид"].Value?.ToString().Equals(domainSearch, StringComparison.OrdinalIgnoreCase) == true;

                if (matchesText && matchesDomain)
                {
                    row.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }

            MessageBox.Show("Ничего не найдено");
        }
        private void button5_Click(object sender, EventArgs e)
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
                    var vid = row.Cells["Вид"].Value;
                    var name = row.Cells["Название"].Value;
                    var kol = row.Cells["Количество"].Value;
                    string query = "UPDATE osnova SET Вид=@vid, Название=@name, Количество=@kol WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());                   
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@vid", vid);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@kol", kol);
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
    }
}
