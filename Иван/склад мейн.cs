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
    public partial class склад : Form
    {
        DB db = new DB();
        private void склад_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        public склад()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            мейн form5 = new мейн();
            form5.Show();
            this.Hide();
        }  
        private void button2_Click(object sender, EventArgs e)
        {
            Ред_склад form3 = new Ред_склад();
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

        private void button3_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();
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
    }
}
