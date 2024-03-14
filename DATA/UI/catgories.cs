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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace DATA.UI
{
    public partial class catgories : Form
    {
        private readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\user\Documents\roopmukherjee.mdb";
        public catgories()
        {
            InitializeComponent();
            LoadCategoriesWithItemCount();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            dashfrm dashfrm = new dashfrm();
            dashfrm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            items items = new items();
            items.Show();
            this.Hide();

        }

        private void label4_Click(object sender, EventArgs e)
        {
            billing billing = new billing();
            billing.Show();
            this.Hide();
        }

        private void catgories_Load(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\user\Documents\roopmukherjee.mdb";
                connection.Open();
                cheak.Text = "connection successful";
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string valueToAdd = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(valueToAdd))
            {
                MessageBox.Show("Please enter a value to add.");
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO categories (categoryname) VALUES (textBox1.Text)";

                    using (OleDbCommand command = new OleDbCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("textBox1.Text", valueToAdd);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Value added successfully.");
                            LoadDataIntoDataGridView(); // Reload data into the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Failed to add value.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadDataIntoDataGridView()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM categories"; 
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dataTable;
                        }
                        else
                        {
                            MessageBox.Show("No data found in the database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Retrieve the category name from the selected row
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                string categoryName = dataGridView1.Rows[selectedRowIndex].Cells["categoryname"].Value.ToString(); // Assuming "categoriesname" is the primary key column name

                // Confirm with the user before deleting
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(connectionString))
                        {
                            connection.Open();
                            string deleteQuery = "DELETE FROM categories WHERE categoryname = @CategoryName"; // Adjust table name and primary key column name
                            using (OleDbCommand command = new OleDbCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("@CategoryName", categoryName);
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record deleted successfully.");
                                    LoadDataIntoDataGridView(); // Refresh DataGridView
                                }
                                else
                                {
                                    MessageBox.Show("Failed to delete record.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void LoadCategoriesWithItemCount()
        {
            listBox1.Items.Clear(); // Clear existing items

            try
            {
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
                                string displayText = $"{categoryName,-30} ({itemCount})"; // Align item count to the right
                                listBox1.Items.Add(displayText);
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
