using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DATA.UI
{
    public partial class dashfrm : Form
    {
        private readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\user\Documents\roopmukherjee.mdb";
        public dashfrm()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            catgories catgories = new catgories();
            catgories.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            items items = new items();
            items.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            billing billing = new billing();
            billing.Show();
            this.Hide();
        }

        private void dashfrm_Load(object sender, EventArgs e)
        {
            LoadCategoriesWithItemCount();


        }
        private void LoadCategoriesWithItemCount()
        {
            try
            {
                // Clear existing series
                chart1.Series.Clear();

                // Add new series
                Series series = chart1.Series.Add("ItemCounts");
                series.ChartType = SeriesChartType.Column;

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT categoryname, COUNT(*) AS itemcount FROM items GROUP BY categoryname ORDER BY categoryname";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string categoryName = reader["categoryname"].ToString();
                                int itemCount = Convert.ToInt32(reader["itemcount"]);
                                series.Points.AddXY(categoryName, itemCount);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void label11_Click_1(object sender, EventArgs e)
        {

        }


        private void LoadCategories()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT categoryname FROM categories";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string categoryName = reader["categoryname"].ToString();
                                comboBox1.Items.Add(categoryName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string categoryName = comboBox1.SelectedItem?.ToString();
            string itemName = textBox1.Text.Trim();

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT itemsname, manufacture, price, stock FROM items WHERE categoryname = @CategoryName AND itemsname LIKE @ItemName";
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@CategoryName", categoryName);
                        adapter.SelectCommand.Parameters.AddWithValue("@ItemName", "%" + itemName + "%");
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        
    }
    }
    }

