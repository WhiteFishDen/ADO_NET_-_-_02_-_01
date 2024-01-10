using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;


namespace ADO_NET_ДЗ_Модуль_02_часть_01
{
    public partial class Form1 : Form
    {
        private string connection_string;
        private NpgsqlConnection connection;
        private string table_name;
        public Form1()
        {
            InitializeComponent();
            connection_string = "Host=localhost;" +
                "Username=postgres;" +
                "Password=123;" +
                "Database=warehouse;";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new NpgsqlConnection(connection_string);
            connection.Open();
           
        }
        private DataTable loadTable()
        {
            DataTable storage_dt = new DataTable();
            NpgsqlDataAdapter sda = new NpgsqlDataAdapter($"SELECT * FROM {table_name}", connection);
            sda.Fill(storage_dt);
            return storage_dt;
        }
        
        private void btn_execute_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) insertToProducts();
            if (radioButton2.Checked) insertToProviders();
            if (radioButton3.Checked) insertToCategory();
        }
        private void btn_update_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) updateToProducts();
            if (radioButton2.Checked) updateToProviders();
            if (radioButton3.Checked) updateToCategory();
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) DeleteData();
            if (radioButton2.Checked) DeleteData();
            if (radioButton3.Checked) DeleteData();
        }

        private void insertToProducts()
        {
            string commandText = $"INSERT INTO {table_name} (_name, _price, category_id)" +
               $" VALUES (@name,@price,@category_id)";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.Parameters.AddWithValue("price", Convert.ToDecimal(textBox2.Text));
                cmd.Parameters.AddWithValue("category_id", Convert.ToInt32(textBox3.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been inserted successfully!");
                dataGridView1.DataSource = loadTable();
            }
        }
        private void insertToProviders()
        {
            string commandText = $"INSERT INTO {table_name} (_name, _phone_number)" +
               $" VALUES (@name,@phone_number)";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.Parameters.AddWithValue("phone_number", Convert.ToInt64(textBox2.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been inserted successfully!");
                dataGridView1.DataSource = loadTable();
            }
        }

        private void insertToCategory()
        {
            string commandText = $"INSERT INTO {table_name} (_name)" +
               $" VALUES (@name)";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been inserted successfully!");
                dataGridView1.DataSource = loadTable();
            }
        }
        private void updateToProducts()
        {
            string commandText = $"UPDATE {table_name} SET _name = @name, _price = @price, category_id = @category_id" +
                $" WHERE id = @id";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", Convert.ToInt16(textBox4.Text));
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.Parameters.AddWithValue("price", Convert.ToDecimal(textBox2.Text));
                cmd.Parameters.AddWithValue("category_id", Convert.ToInt32(textBox3.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been updated successfully!");
                dataGridView1.DataSource = loadTable();
            }
        }
        private void updateToProviders()
        {
            string commandText = $"UPDATE {table_name} SET _name = @name, _phone_number = @phone_number" +
                $" WHERE id = @id";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", Convert.ToInt16(textBox4.Text));
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.Parameters.AddWithValue("phone_number", Convert.ToInt64(textBox2.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been updated successfully!");
                dataGridView1.DataSource = loadTable();
            }
        }
        private void updateToCategory()
        {
            string commandText = $"UPDATE {table_name} SET _name = @name WHERE id = @id";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", Convert.ToInt16(textBox4.Text));
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been updated successfully!");
                dataGridView1.DataSource = loadTable();
            }
        }

        private void DeleteData()
        {
            string commandText = $"DELETE FROM {table_name} WHERE id = @id";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", Convert.ToInt16(textBox4.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been deleted successfully!");
                dataGridView1.DataSource = loadTable();
            }
        }

        private void btn_choice_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                ClearAllText();
                table_name = "products";
                dataGridView1.DataSource = loadTable();
                label1.Text = "Name product";
                textBox1.Enabled = true;
                label2.Text = "Price";
                textBox2.Enabled = true;
                label3.Text = "Category";
                textBox3.Enabled = true;
                
            }
            if(radioButton2.Checked)
            {
                ClearAllText();
                table_name = "providers";
                dataGridView1.DataSource = loadTable();
                label1.Text = "Name provider";
                textBox1.Enabled = true;
                label2.Text = "Phone number";
                textBox2.Enabled = true;
                label3.Text = "";
                textBox3.Enabled = false;
                
            }
            if(radioButton3.Checked)
            {
                ClearAllText();
                table_name = "category_product";
                dataGridView1.DataSource = loadTable();
                label1.Text = "Category Name";
                textBox1.Enabled = true;
                label2.Text = "";
                textBox2.Enabled = false;
                label3.Text = "";
                textBox3.Enabled = false;
                
            }
        }
        private void ClearAllText()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

       
    }
    
}
