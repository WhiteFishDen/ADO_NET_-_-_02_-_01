using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
            try
            {
                connection = new NpgsqlConnection(connection_string);
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + '\n' + "\tCheck the entered data for connection!", "Error!");
            }
        }
        private DataTable loadTable (string table, string command = "SELECT * FROM ")
        {
            if (command == "SELECT * FROM ") command += table;
            try
            {
                DataTable storage_dt = new DataTable();
                NpgsqlDataAdapter sda = new NpgsqlDataAdapter(command, connection);
                sda.Fill(storage_dt);
                return storage_dt;
            }
            catch ( Exception ex )
            {
                
                MessageBox.Show(ex.Message, "Error!");
                return null;
            }
           
        }
        
        private void btn_execute_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked) insertToProducts();
                if (radioButton2.Checked) insertToProviders();
                if (radioButton3.Checked) insertToCategory();
                if (radioButton9.Checked) insertToProducts_Providers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
            
        }
        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked) updateToProducts();
                if (radioButton2.Checked) updateToProviders();
                if (radioButton3.Checked) updateToCategory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
           
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked) DeleteData();
                if (radioButton2.Checked) DeleteData();
                if (radioButton3.Checked) DeleteData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
            
        }

        private void insertToProducts_Providers()
        {
            string commandText = $"INSERT INTO {table_name} (products_id, providers_id)" +
                $" VALUES (@prodID, @provID)";
            using(var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("prodID", Convert.ToInt16(textBox1.Text));
                cmd.Parameters.AddWithValue("provID", Convert.ToInt16(textBox2.Text));
                cmd.ExecuteNonQuery();
                dataGridView1.DataSource = loadTable(table_name);
            }
        }

        private void insertToProducts()
        {
            string commandText = $"INSERT INTO {table_name} (_name, _price, category_id, date_of_delivery)" +
               $" VALUES (@name,@price,@category_id, @date)";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.Parameters.AddWithValue("price", Convert.ToDecimal(textBox2.Text));
                cmd.Parameters.AddWithValue("category_id", Convert.ToInt32(textBox3.Text));
                cmd.Parameters.AddWithValue("date", dateTimePicker1.Value);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been inserted successfully!");
                dataGridView1.DataSource = loadTable(table_name);
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
                dataGridView1.DataSource = loadTable(table_name);
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
                dataGridView1.DataSource = loadTable(table_name);
            }
        }



        private void updateToProducts()
        {
                string commandText = $"UPDATE {table_name} SET _name = @name, _price = @price, category_id = @category_id, date_of_delivery = @date" +
              $" WHERE id = @id";
                using (var cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("id", Convert.ToInt16(textBox4.Text));
                    cmd.Parameters.AddWithValue("name", textBox1.Text);
                    cmd.Parameters.AddWithValue("price", Convert.ToDecimal(textBox2.Text));
                    cmd.Parameters.AddWithValue("category_id", Convert.ToInt32(textBox3.Text));
                    cmd.Parameters.AddWithValue("date", dateTimePicker1.Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data has been updated successfully!");
                    dataGridView1.DataSource = loadTable(table_name);
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
                dataGridView1.DataSource = loadTable(table_name);
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
                dataGridView1.DataSource = loadTable(table_name);
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
                dataGridView1.DataSource = loadTable(table_name);
            }
        }
        private void btn_choice_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                ClearAllText();
                table_name = "products";
                dataGridView1.DataSource = loadTable(table_name);
                label1.Text = "Name product";
                textBox1.Enabled = true;
                label2.Text = "Price";
                textBox2.Enabled = true;
                label3.Text = "Category";
                textBox3.Enabled = true;
                label5.Text = "Date of delivary";
                dateTimePicker1.Enabled = true;
                
            }
            if(radioButton2.Checked)
            {
                ClearAllText();
                table_name = "providers";
                dataGridView1.DataSource = loadTable(table_name);
                label1.Text = "Name provider";
                textBox1.Enabled = true;
                label2.Text = "Phone number";
                textBox2.Enabled = true;
                label3.Text = "";
                textBox3.Enabled = false;
                label5.Text = "";
                dateTimePicker1.Enabled = false;
            }
            if(radioButton3.Checked)
            {
                ClearAllText();
                table_name = "category_product";
                dataGridView1.DataSource = loadTable(table_name);
                label1.Text = "Category Name";
                textBox1.Enabled = true;
                label2.Text = "";
                textBox2.Enabled = false;
                label3.Text = "";
                textBox3.Enabled = false;
                label5.Text = "";
                dateTimePicker1.Enabled = false;
            }
            if(radioButton9.Checked)
            {
                ClearAllText();
                table_name = "products_providers";
                dataGridView1.DataSource = loadTable(table_name);
                label1.Text = "products_id";
                textBox1.Enabled = true;
                label2.Text = "providers_id";
                textBox2.Enabled = true;
                label3.Text = "";
                textBox3.Enabled = false;
                label5.Text = "";
                dateTimePicker1.Enabled = false;
            }
        }
        private void ClearAllText()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            
        }
      
        
        private void btn_choice_select_Click(object sender, EventArgs e)
        {
            if(radioButton4.Checked)
            {
                dataGridView1.DataSource = loadTable(table_name,"select providers._name, providers._phone_number, count(products_id) as \"Quantit products\"" +
                    " from providers inner join products_providers on providers.id = providers_id join products on products_id = products.id" +
                    " group by providers._name, providers._phone_number order by count(products_id) desc limit 1;");
            }
            if(radioButton5.Checked)
            {
                dataGridView1.DataSource = loadTable(table_name,"select providers._name, providers._phone_number, count(products_id) as \"Quantity products\"" +
                    "from providers inner join products_providers on providers.id = providers_id join products on products_id = products.id" +
                    " group by providers._name, providers._phone_number order by count(products_id) limit 1;");
            }
            if(radioButton6.Checked)
            {
                dataGridView1.DataSource = loadTable(table_name, "select category_product._name as \"Name of category\", count(products.id) as \"Quantity of products\" " +
                    "from category_product join products on category_product.id = products.category_id group by category_product._name order by count(products.id) desc limit 1;");
            }
            if(radioButton7.Checked)
            {
                dataGridView1.DataSource = loadTable(table_name, "select category_product._name as \"Name of category\", count(products.id) as \"Quantity of products\" "+
                    "from category_product join products on category_product.id = products.category_id group by category_product._name order by count(products.id) limit 1;");
            }
            if(radioButton8.Checked)
            {
                dataGridView1.DataSource = loadTable(table_name, $"select * from products as p where now() >= p.date_of_delivery + {textBox5.Text}");
            }
        }

    }
    
}
