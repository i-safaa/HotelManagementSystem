using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class StaffForm : Form
    {
        public StaffForm()
        {
            InitializeComponent();
        }

        string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Hotel_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public void poplate()
        {
            string query = "select * from Staff_tbl";
            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        guna2DataGridView1.DataSource = dataTable;
                    }
                }
            }
        }

        private void StaffForm_Load(object sender, EventArgs e)
        {
            poplate();
            label2.Text = DateTime.Now.ToShortDateString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox3.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox4.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox1.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "ننبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "insert into Staff_tbl (Staff_id,Staffname,Staffphone,gender,Staffpassword) values (@Staff_id,@Staffname,@Staffphone,@gender,@Staffpassword)";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Staff_id", bunifuMaterialTextbox1.Text);
                        command.Parameters.AddWithValue("@Staffname", bunifuMaterialTextbox2.Text);
                        command.Parameters.AddWithValue("@Staffphone", bunifuMaterialTextbox3.Text);
                        command.Parameters.AddWithValue("@Staffpassword", bunifuMaterialTextbox4.Text);
                        command.Parameters.AddWithValue("@gender", comboBox1.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("تمت الاضافة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        poplate();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // التأكد أن المستخدم لم يضغط على رأس الجدول
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                bunifuMaterialTextbox1.Text = row.Cells["Staff_id"].Value.ToString();
                bunifuMaterialTextbox2.Text = row.Cells["Staffname"].Value.ToString();
                bunifuMaterialTextbox3.Text = row.Cells["Staffphone"].Value.ToString();
                bunifuMaterialTextbox4.Text = row.Cells["Staffpassword"].Value.ToString();
                comboBox1.Text = row.Cells["gender"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox3.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox4.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox1.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "ننبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "update  Staff_tbl set Staffname = @Staffname , Staffphone = @Staffphone , gender = @gender , Staffpassword = @Staffpassword where Staff_id = @Staff_id ";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Staff_id", bunifuMaterialTextbox1.Text);
                        command.Parameters.AddWithValue("@Staffname", bunifuMaterialTextbox2.Text);
                        command.Parameters.AddWithValue("@Staffphone", bunifuMaterialTextbox3.Text);
                        command.Parameters.AddWithValue("@Staffpassword", bunifuMaterialTextbox4.Text);
                        command.Parameters.AddWithValue("@gender", comboBox1.Text);

                        int rowsAffected = Convert.ToInt32(command.ExecuteNonQuery());
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("تمت التحديث بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("المعرف غير موجود في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        poplate();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
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

                    string query = @"Delete from Staff_tbl where Staff_id = @Staff_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Staff_id", bunifuMaterialTextbox1.Text);

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

                    poplate();
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
            string query = "select * from Staff_tbl where Staffname like @Staffname or gender like @gender";
            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Staffname", bunifuMaterialTextbox5.Text.Trim() + "%");
                    command.Parameters.AddWithValue("@gender", bunifuMaterialTextbox5.Text.Trim() + "%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            poplate();
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            HomeForm homeForm = new HomeForm();
            this.Hide();
            homeForm.Show();
        }
    }
}
