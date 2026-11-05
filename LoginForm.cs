using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()) || string.IsNullOrWhiteSpace(bunifuMaterialTextbox2.Text.Trim()))
            {
                MessageBox.Show("ادخل اسم المستخدم وكلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "select COUNT(*) FROM Staff_tbl where Staffname = @Staffname and Staffpassword = @Staffpassword";
            string Connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Hotel_db.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection con = new SqlConnection(Connection))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Staffname", bunifuMaterialTextbox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Staffpassword", bunifuMaterialTextbox2.Text.Trim());

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("تم تسجيل الدخول بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        HomeForm homeForm = new HomeForm();
                        this.Hide();
                        homeForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("خطا في اسم المستخدم او كلمة المرور", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }

        }
    }
}
