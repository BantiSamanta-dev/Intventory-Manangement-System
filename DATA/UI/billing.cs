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

namespace DATA.UI
{
    public partial class billing : Form
    {

        private readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\user\Documents\roopmukherjee.mdb";
        public billing()
        {
            InitializeComponent();
           
           

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            items items = new items();
            items.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            catgories  catgories = new catgories();
            catgories.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            dashfrm  dashfrm = new dashfrm();
            dashfrm.Show();
            this.Hide();
        }

        private void billing_Load(object sender, EventArgs e)
        {
             LoadCategories();
            LoadItems(comboBox1.SelectedItem?.ToString());

        }

        // Populate the ComboBox with categories
        private void LoadCategories()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT categoryname FROM items ORDER BY categoryname";
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

        // Load items based on selected category
        private void LoadItems(string categoryName)
        {
            // Clear existing items
            comboBox2.Items.Clear();

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT itemsname FROM items WHERE categoryname = @CategoryName ORDER BY itemsname";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Provide a default value if categoryName is null
                        command.Parameters.AddWithValue("@CategoryName", categoryName ?? DBNull.Value.ToString());
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string itemName = reader["itemsname"].ToString();
                                comboBox2.Items.Add(itemName);
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

        private void CalculateTotalPrice()
        {
            // Get selected item and quantity
            string itemName = comboBox2.SelectedItem?.ToString();
            int quantity = (int)numericUpDown2.Value;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT price FROM items WHERE itemsname = @ItemName";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            decimal price = Convert.ToDecimal(result);
                            decimal totalPrice = price * quantity;
                            totalprice.Text = totalPrice.ToString("0.00");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Event handler for category selection change
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = comboBox1.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                LoadItems(selectedCategory);
            }
        }

        // Event handler for item selection change
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }

        // Event handler for quantity change
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }
    }
}