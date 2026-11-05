using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class RoomForm : Form
    {
        public RoomForm()
        {
            InitializeComponent();
        }

        string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Hotel_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public void populate()
        {
            string query = "select * from Room_tbl";
            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;

                        // بعد تعيين الـ DataSource للـ DataGridView، أضف هذا الكود:
                        guna2DataGridView1.Columns["Room_number"].HeaderText = "RoomNumber";
                        guna2DataGridView1.Columns["Roomphone"].HeaderText = "RoomPhone";
                        guna2DataGridView1.Columns["Roomstate"].HeaderText = "RoomState";
                    }
                }
            }
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {
            populate();
            label2.Text = DateTime.Now.ToShortDateString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox1.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "ننبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "insert into Room_tbl (Room_number ,Roomphone,Roomstate) values (@Room_number,@Roomphone,@Roomstate)";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Room_number", bunifuMaterialTextbox1.Text.Trim());
                        command.Parameters.AddWithValue("@Roomphone", bunifuMaterialTextbox2.Text.Trim());
                        command.Parameters.AddWithValue("@Roomstate", comboBox1.Text.Trim());

                        command.ExecuteNonQuery();
                        MessageBox.Show("تمت الاضافة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        populate();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox1.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "ننبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "Update Room_tbl set Roomphone = @Roomphone , Roomstate = @Roomstate where Room_number = @Room_number";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Room_number", bunifuMaterialTextbox1.Text.Trim());
                        command.Parameters.AddWithValue("@Roomphone", bunifuMaterialTextbox2.Text.Trim());
                        command.Parameters.AddWithValue("@Roomstate", comboBox1.Text.Trim());

                        int rowAff = Convert.ToInt32(command.ExecuteNonQuery());

                        if (rowAff > 0)
                        {
                            MessageBox.Show("تم التحديث بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("المعرف غير موجود في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        populate();
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

                bunifuMaterialTextbox1.Text = row.Cells["Room_number"].Value.ToString();
                bunifuMaterialTextbox2.Text = row.Cells["Roomphone"].Value.ToString();
                comboBox1.Text = row.Cells["Roomstate"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()))
            {
                MessageBox.Show("ادخل المعرف ", "ننبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "delete from Room_tbl where Room_number = @Room_number";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Room_number", bunifuMaterialTextbox1.Text.Trim());

                        int rowAff = Convert.ToInt32(command.ExecuteNonQuery());

                        if (rowAff > 0)
                        {
                            MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("المعرف غير موجود في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        populate();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            HomeForm homeForm = new HomeForm();
            this.Hide();
            homeForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = "select * from Room_tbl where Roomstate like @Roomstate";

            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Roomstate", bunifuMaterialTextbox5.Text.Trim());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;
                    }
                }
            }
        }
    }
}
