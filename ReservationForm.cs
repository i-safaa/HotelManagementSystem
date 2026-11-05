using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagementSystem
{
    public partial class ReservationForm : Form
    {
        public ReservationForm()
        {
            InitializeComponent();
        }

        string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lenovo\Documents\Hotel_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        bool isLoading = false;

        public void populate()
        {
            string query = "select * from Reservation_tbl";

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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (dateTimePicker1.Value.Date < DateTime.Today || dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                isLoading = true;
                try
                {
                    dateTimePicker1.Value = DateTime.Today;
                    if (dateTimePicker2.Value.Date < dateTimePicker1.Value.Date)
                    {
                        dateTimePicker2.Value = dateTimePicker1.Value;
                    }
                }
                finally
                {
                    isLoading = false;
                }
                MessageBox.Show("Wrong Date For Reservation");
                return;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (dateTimePicker2.Value.Date < dateTimePicker1.Value.Date)
            {
                isLoading = true;
                try
                {
                    dateTimePicker2.Value = dateTimePicker1.Value;
                }
                finally
                {
                    isLoading = false;
                }
                MessageBox.Show("Wrong Dateout. check once more");
                return;
            }
        }

        public void fillClientName()
        {
            string query = "select Client_name from Client_tbl";

            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        comboBox1.DataSource = dataTable;
                        comboBox1.DisplayMember = "Client_name";
                    }
                }
            }
        }

        public void fillRoomNumber()
        {
            string query = "select Room_number from Room_tbl where Roomstate = @Roomstate";

            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Roomstate", "free");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        comboBox2.DataSource = dataTable;
                        comboBox2.DisplayMember = "Room_number";

                        if (dataTable.Rows.Count == 0)
                        {
                            comboBox2.Text = "";
                        }
                    }
                }
            }
        }

        private void ReservationForm_Load(object sender, EventArgs e)
        {
            fillClientName();
            fillRoomNumber();
            populate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox2.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int resId;
            int roomNumber;

            if (!int.TryParse(bunifuMaterialTextbox1.Text.Trim(), out resId))
            {
                MessageBox.Show("المعرف يجب ان يكون رقما", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(comboBox2.Text.Trim(), out roomNumber))
            {
                MessageBox.Show("رقم الغرفة غير صحيح", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "insert into Reservation_tbl (Res_id,Resclient,Resroom,Resdatein,Resdateout) values (@Res_id,@Resclient,@Resroom,@Resdatein,@Resdateout)";

            string updateRoomQuery = "update Room_tbl set Roomstate = 'buys' where Room_number = @Room_number";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Res_id", resId);
                                command.Parameters.AddWithValue("@Resclient", comboBox1.Text.Trim());
                                command.Parameters.AddWithValue("@Resroom", roomNumber.ToString());
                                command.Parameters.AddWithValue("@Resdatein", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                                command.Parameters.AddWithValue("@Resdateout", dateTimePicker2.Value.ToString("yyyy-MM-dd"));

                                command.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdRoom = new SqlCommand(updateRoomQuery, connection, transaction))
                            {
                                cmdRoom.Parameters.AddWithValue("@Room_number", roomNumber);
                                cmdRoom.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    MessageBox.Show("تمت الاضافة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    populate();
                    fillRoomNumber();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox1.Text.Trim()) ||
                string.IsNullOrWhiteSpace(comboBox2.Text.Trim()))
            {
                MessageBox.Show("ادخل جميع الاعمدة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int resId;
            int newRoomNumber;

            if (!int.TryParse(bunifuMaterialTextbox1.Text.Trim(), out resId))
            {
                MessageBox.Show("المعرف يجب ان يكون رقما", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(comboBox2.Text.Trim(), out newRoomNumber))
            {
                MessageBox.Show("رقم الغرفة غير صحيح", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string oldRoom = "";
            string getOldRoomQuery = "select Resroom from Reservation_tbl where Res_id = @Res_id";

            string query = "update Reservation_tbl set Resclient = @Resclient , Resroom = @Resroom , Resdatein = @Resdatein , Resdateout = @Resdateout where Res_id = @Res_id";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmdGet = new SqlCommand(getOldRoomQuery, connection, transaction))
                            {
                                cmdGet.Parameters.AddWithValue("@Res_id", resId);

                                object result = cmdGet.ExecuteScalar();

                                if (result != null && result != DBNull.Value)
                                {
                                    oldRoom = result.ToString();
                                }
                            }

                            int rowAff;

                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Res_id", resId);
                                command.Parameters.AddWithValue("@Resclient", comboBox1.Text.Trim());
                                command.Parameters.AddWithValue("@Resroom", newRoomNumber.ToString());
                                command.Parameters.AddWithValue("@Resdatein", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                                command.Parameters.AddWithValue("@Resdateout", dateTimePicker2.Value.ToString("yyyy-MM-dd"));

                                rowAff = command.ExecuteNonQuery();
                            }

                            if (rowAff > 0)
                            {
                                int oldRoomNumber;

                                if (int.TryParse(oldRoom, out oldRoomNumber) && oldRoomNumber != newRoomNumber)
                                {
                                    string freeOld = "update Room_tbl set Roomstate = 'free' where Room_number = @OldRoom";

                                    using (SqlCommand cmdFree = new SqlCommand(freeOld, connection, transaction))
                                    {
                                        cmdFree.Parameters.AddWithValue("@OldRoom", oldRoomNumber);
                                        cmdFree.ExecuteNonQuery();
                                    }

                                    string setBuysNew = "update Room_tbl set Roomstate = 'buys' where Room_number = @NewRoom";

                                    using (SqlCommand cmdBuys = new SqlCommand(setBuysNew, connection, transaction))
                                    {
                                        cmdBuys.Parameters.AddWithValue("@NewRoom", newRoomNumber);
                                        cmdBuys.ExecuteNonQuery();
                                    }
                                }

                                transaction.Commit();

                                MessageBox.Show("تم التحديث بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                populate();
                                fillRoomNumber();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("المعرف غير موجود في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                bunifuMaterialTextbox1.Text = Convert.ToString(row.Cells["Res_id"].Value);
                comboBox1.Text = Convert.ToString(row.Cells["Resclient"].Value);
                comboBox2.Text = Convert.ToString(row.Cells["Resroom"].Value);

                isLoading = true;
                try
                {
                    DateTime dateIn = Convert.ToDateTime(row.Cells["Resdatein"].Value);
                    DateTime dateOut = Convert.ToDateTime(row.Cells["Resdateout"].Value);

                    dateTimePicker1.Value = dateIn;
                    dateTimePicker2.Value = dateOut;
                }
                finally
                {
                    isLoading = false;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextbox1.Text.Trim()))
            {
                MessageBox.Show("ادخل المعرف ", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int resId;

            if (!int.TryParse(bunifuMaterialTextbox1.Text.Trim(), out resId))
            {
                MessageBox.Show("المعرف يجب ان يكون رقما", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string getRoomQuery = "select Resroom from Reservation_tbl where Res_id = @Res_id";

            string query = "delete from Reservation_tbl where Res_id = @Res_id";

            string freeRoomQuery = "update Room_tbl set Roomstate = 'free' where Room_number = @Room_number";

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string roomToFree = "";

                            using (SqlCommand cmdGet = new SqlCommand(getRoomQuery, connection, transaction))
                            {
                                cmdGet.Parameters.AddWithValue("@Res_id", resId);

                                object result = cmdGet.ExecuteScalar();

                                if (result != null && result != DBNull.Value)
                                {
                                    roomToFree = result.ToString();
                                }
                            }

                            int rowAff;

                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Res_id", resId);

                                rowAff = command.ExecuteNonQuery();
                            }

                            if (rowAff > 0)
                            {
                                int roomNumber;

                                if (int.TryParse(roomToFree, out roomNumber))
                                {
                                    using (SqlCommand cmdFree = new SqlCommand(freeRoomQuery, connection, transaction))
                                    {
                                        cmdFree.Parameters.AddWithValue("@Room_number", roomNumber);
                                        cmdFree.ExecuteNonQuery();
                                    }
                                }

                                transaction.Commit();

                                MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("المعرف غير موجود في قاعدة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    populate();
                    fillRoomNumber();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطا: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            populate();
            fillRoomNumber();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = "select * from Reservation_tbl where Resclient like @Resclient or Resroom like @Resroom";

            using (SqlConnection connection = new SqlConnection(con))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    string searchVal = "%" + bunifuMaterialTextbox5.Text.Trim() + "%";

                    command.Parameters.AddWithValue("@Resclient", searchVal);
                    command.Parameters.AddWithValue("@Resroom", searchVal);

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