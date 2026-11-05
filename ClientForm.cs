using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Hotel_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public void populate()
        {
            using (SqlConnection connection = new SqlConnection(con))
            {
                string query = "select * from Client_tbl";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;
                        guna2DataGridView1.ReadOnly = true;
                    }
                }
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            populate();
            label2.Text = DateTime.Now.ToShortDateString();
        }

        private void guna2DataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // التأكد أن المستخدم لم يضغط على رأس الجدول
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                bunifuMaterialTextbox1.Text = row.Cells["Client_id"].Value.ToString();
                bunifuMaterialTextbox2.Text = row.Cells["Client_name"].Value.ToString();
                bunifuMaterialTextbox3.Text = row.Cells["Client_phone"].Value.ToString();
                comboBox1.Text = row.Cells["Client_country"].Value.ToString();
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim())
                || string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text.Trim())
                || string.IsNullOrWhiteSpace(bunifuMaterialTextbox3.Text.Trim())
                || string.IsNullOrWhiteSpace(comboBox1.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "ننبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    string query = @"insert into Client_tbl (Client_id , Client_name , Client_phone , Client_country) values (@Client_id , @Client_name , @Client_phone , @Client_country)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Client_id", bunifuMaterialTextbox1.Text);
                        command.Parameters.AddWithValue("@Client_name", bunifuMaterialTextbox2.Text);
                        command.Parameters.AddWithValue("@Client_phone", bunifuMaterialTextbox3.Text);
                        command.Parameters.AddWithValue("@Client_country", comboBox1.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("تمت الاضافة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    populate();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim())
                || string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text.Trim())
                || string.IsNullOrWhiteSpace(bunifuMaterialTextbox3.Text.Trim())
                || string.IsNullOrWhiteSpace(comboBox1.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "ننبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    string query = @"Update Client_tbl set Client_name = @Client_name , Client_phone = @Client_phone , Client_country = @Client_country where Client_id = @Client_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Client_id", bunifuMaterialTextbox1.Text);
                        command.Parameters.AddWithValue("@Client_name", bunifuMaterialTextbox2.Text);
                        command.Parameters.AddWithValue("@Client_phone", bunifuMaterialTextbox3.Text);
                        command.Parameters.AddWithValue("@Client_country", comboBox1.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("تم التحديث بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("المعرف غير موجود في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                    populate();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()))
            {
                MessageBox.Show("الرجاء إدخال المعرف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    string query = @"Delete from Client_tbl where Client_id = @Client_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Client_id", bunifuMaterialTextbox1.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("المعرف غير موجود في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    populate();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = "select * from Client_tbl where Client_name like @Client_name or Client_country like @Client_country";
            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Client_name", "%" + bunifuMaterialTextbox4.Text.Trim() + "%");
                    command.Parameters.AddWithValue("@Client_country", "%" + bunifuMaterialTextbox4.Text.Trim() + "%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.ReadOnly = true;
                        guna2DataGridView1.DataSource = dt;
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            populate();
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            this.Hide();
            home.Show();
        }
    }
}
